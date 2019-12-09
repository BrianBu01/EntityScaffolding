using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding
{
    internal static class ConventionFinder
    {
        public static IEnumerable<IWritableElement> FindApplicableConventions<TWritableElement>(this IEnumerable<IConventionMatcher<TWritableElement>> conventions,
            IEntityType entityType, string @namespace) where TWritableElement : IWritableElement
        {
            return conventions.SelectMany(entityConvention => 
                entityConvention.GetMatchingElements(entityType, @namespace)).
                Cast<IWritableElement>();
        }

    }
}
