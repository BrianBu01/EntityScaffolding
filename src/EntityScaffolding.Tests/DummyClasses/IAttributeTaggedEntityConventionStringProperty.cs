using System.Collections.Generic;
using System.Text;

namespace EntityScaffolding.Tests.DummyClasses
{
    [EntityConvention]
    public interface IAttributeTaggedEntityConventionStringProperty
    {
        string CustomProperty { get; set; }
    }
}
