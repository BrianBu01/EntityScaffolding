namespace EntityScaffolding.Tests.DummyClasses
{
    [EntityConvention]
    public interface IAttributeTaggedEntityConventionNullableIntProperty
    {
        int? CustomProperty { get; set; }
    }
}