using System;
using System.Linq;
using EntityScaffolding.Elements;

namespace EntityScaffolding.Editors
{
    public class PropertyAttributeEditor : EntityEditor<PropertyAttributeElement>
    {
        
        protected override string EditEntityBase(string entitySource)
        {
            var lineSpacing = $"{Environment.NewLine}        ";

            if (!WritableElements.Any()) return entitySource;

            foreach (var grouped in WritableElements.GroupBy(x => x.Property))
            {
                var property = grouped.Key;
                var propertyDefinition = $"public {TypeNameWriter.GetTypeName(property.ClrType)} {property.Name}";

                var attributeNames = from attribute in grouped
                    let ctorParameters = attribute.AttributeValues.Any()
                        ? $"({string.Join(", ", attribute.AttributeValues)})"
                        : string.Empty
                    let typeName = TypeNameWriter.GetTypeName(attribute.Attribute)
                    let attributeName = typeName.Replace("Attribute", string.Empty)
                    select $"[{attributeName}{ctorParameters}]";

                var attributes = string.Join(lineSpacing, attributeNames);

                entitySource = entitySource.Replace(propertyDefinition,
                    $"{lineSpacing}{attributes}{lineSpacing}{propertyDefinition}");
            }

            return entitySource;
        }
    }
}