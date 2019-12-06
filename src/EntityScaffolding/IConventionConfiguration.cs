using System.Collections.Generic;
using EntityScaffolding.Editors;
using EntityScaffolding.Elements;
using EntityScaffolding.Matchers;

namespace EntityScaffolding
{
    public interface IConventionConfiguration
    {
        IEnumerable<IConventionMatcher<IWritableElement>> ConventionMatchers { get; }

        IReadOnlyList<IEntityEditor> EntityEditors { get; }
    }
}