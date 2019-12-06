using System;

namespace EntityScaffolding.Tests.DummyClasses
{
    [EntityConvention]
    public interface IAttributeTaggedEntityConventionDateTimeProperty
    {
        DateTime CustomProperty { get; set; }
    }
}