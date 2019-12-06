using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.DefaultConventions.Matchers
{
    public class PrimaryKeyPropertyConventionMatcher : IPropertyAttributeConventionMatcher
    {
        public IEnumerable<PropertyAttributeElement> GetMatchingElements(IEntityType entityType)
        {
            var key = entityType.FindPrimaryKey();
            if (key != null && key.Properties.Any())
            {
                if (key.Properties.Count == 1)  //If there is only 1 PK then don't add parameter 0 to it.
                {
                    yield return new PropertyAttributeElement
                        {
                            Attribute = typeof(PrimaryKeyAttribute),
                            Property = key.Properties[0]
                        };
                    yield break;
                }

                foreach (var result in key.Properties.Select((property, index) =>
                    new PropertyAttributeElement
                    {
                        Attribute = typeof(PrimaryKeyAttribute),
                        Property = property,
                        AttributeValues = new List<string>
                        {
                            index.ToString()
                        }
                    }))
                {
                    yield return result;
                }
                
            }
        }
    }
}