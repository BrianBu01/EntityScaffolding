using System;
using System.Linq;
using EntityScaffolding.Elements;

namespace EntityScaffolding.Editors
{
    public class InterfaceEditor : EntityEditor<InterfaceElement>
    {
        protected override string EditEntityBase(string entitySource)
        {
            if (!WritableElements.Any()) return entitySource;

            var className = EntityType.Name;

            var classDefinition = $"public partial class {className}";

            var implementedInterfaces = " : " + string.Join(", ", WritableElements.Select(x => TypeNameWriter.GetTypeName(x.InterfaceType)));

            entitySource = entitySource.Replace(classDefinition, classDefinition + implementedInterfaces);

            return entitySource;
        }
    }
}
