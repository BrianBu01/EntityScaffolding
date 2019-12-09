using System;
using System.Collections.Generic;
using System.Threading;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Tests.Mocking
{
    public class MockEntityInterfaceConventionMatcher : IInterfaceConventionMatcher
    {
        private static readonly AsyncLocal<Func<IEntityType, InterfaceElement>> AsyncLocalAppliesToEntityFunction = new AsyncLocal<Func<IEntityType, InterfaceElement>>();

        public static Func<IEntityType, InterfaceElement> AppliesToEntityFunction
        {
            get => AsyncLocalAppliesToEntityFunction?.Value;
            set => AsyncLocalAppliesToEntityFunction.Value = value;
        }

        public IEnumerable<InterfaceElement> GetMatchingElements(IEntityType entityType, string @namespace)
        {
            InterfaceElement result;
            if (AppliesToEntityFunction == null || (result = AppliesToEntityFunction(entityType)) == null)
            {
                yield break;
            }

            yield return result;
        }
    }
}
