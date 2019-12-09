using System;
using System.Collections.Generic;
using System.Threading;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Tests.Mocking
{
    public class MockEntityPropertyAttributeConventionMatcher : IPropertyAttributeConventionMatcher
    {
        private static readonly AsyncLocal<Func<IEntityType, PropertyAttributeElement>> AsyncLocalAppliesToEntityFunction = new AsyncLocal<Func<IEntityType, PropertyAttributeElement>>();

        public static Func<IEntityType, PropertyAttributeElement> AppliesToEntityFunction
        {
            get => AsyncLocalAppliesToEntityFunction?.Value;
            set => AsyncLocalAppliesToEntityFunction.Value = value;
        }

        public IEnumerable<PropertyAttributeElement> GetMatchingElements(IEntityType entityType, string @namespace)
        {
            PropertyAttributeElement result;
            if (AppliesToEntityFunction == null || (result = AppliesToEntityFunction(entityType)) == null)
            {
                yield break;
            }

            yield return result;
        }
    }
}