using System.Collections.Generic;
using EntityScaffolding.Elements;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Matchers
{
    public interface IConventionMatcher<out TWritableElement> where TWritableElement : IWritableElement
    {
        IEnumerable<TWritableElement> GetMatchingElements(IEntityType entityType, string @namespace);
    }
}
