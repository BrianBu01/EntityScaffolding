using System;
using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.DefaultConventions.Matchers
{
    public class IdentityConventionMatcher : IInterfaceConventionMatcher
    {
        //I bet there is a better way to do this, but would it be less readable?
        private static readonly Dictionary<int, Type> IdentityTypes = new Dictionary<int, Type>
        {
            {1, typeof(IIdentity<>) },
            {2, typeof(IIdentity<,>) },
            {3, typeof(IIdentity<,,>) },
            {4, typeof(IIdentity<,,,>) },
            {5, typeof(IIdentity<,,,,>) },
        };
        
        public IEnumerable<InterfaceElement> GetMatchingElements(IEntityType entityType)
        {
            var key = entityType.FindPrimaryKey();

            if (key == null || !key.Properties.Any()) yield break;

            if (key.Properties.Count > 5)
            {
                throw new InvalidOperationException("I only added support for 5 keys. You can add more generic interfaces if you like.");
            }

            yield return new InterfaceElement(
                IdentityTypes[key.Properties.Count]
                    .MakeGenericType(key.Properties.Select(p => p.ClrType).ToArray()));
        }
    }
}