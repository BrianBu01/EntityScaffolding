using System;
using System.Linq;
using EntityScaffolding.Editors;
using EntityScaffolding.Elements;
using EntityScaffolding.Tests.DummyClasses;
using EntityScaffolding.Tests.Mocking;
using Xunit;

namespace EntityScaffolding.Tests
{
    public class ConventionConfigurationUnitTests
    {
        [Theory]
        [InlineData(typeof(string), "CustomProperty", typeof(IAttributeTaggedEntityConventionStringProperty))]
        [InlineData(typeof(DateTime), "CustomProperty", typeof(IAttributeTaggedEntityConventionDateTimeProperty))]
        [InlineData(typeof(int?), "CustomProperty", typeof(IAttributeTaggedEntityConventionNullableIntProperty))]

        public void CustomInterfaceConventionPickedUppedAndMatchesEntity
            (Type propertyType, string propertyName, Type conventionType)
        {
            var entityMock = MockUtilities.CreateEntityMock(propertyType, propertyName);

            var configuration = new ConventionConfiguration();

            var elements = configuration.ConventionMatchers.FindApplicableConventions(entityMock.Object).ToList();

            Assert.Single(elements);

            Assert.Equal(conventionType, (elements.Single() as InterfaceElement)?.InterfaceType);
        }

        [Fact]
        public void ConventionPickedUppedAndMatchesEntity()
        {
            var entityMock = MockUtilities.CreateEntityMock(typeof(string), "Something");
            
            MockEntityInterfaceConventionMatcher.AppliesToEntityFunction = e =>
                e == entityMock.Object ? new InterfaceElement(typeof(IDummyInterface1)) : null;

            var configuration = new ConventionConfiguration();

            var elements = configuration.ConventionMatchers.FindApplicableConventions(entityMock.Object).ToList();

            Assert.Single(elements);

            Assert.Equal(typeof(IDummyInterface1), (elements.Single() as InterfaceElement)?.InterfaceType);
        }

        [Fact]
        public void ConventionPickedUpped()
        {
            var configuration = new ConventionConfiguration();

            var entityConventions = configuration.ConventionMatchers;

            Assert.True(entityConventions.
                            SingleOrDefault(x => x.GetType() == typeof(MockEntityInterfaceConventionMatcher)) != null);
        }

        [Fact]
        public void DefaultEditorsAreCorrect()
        {
            var configuration = new ConventionConfiguration();

            var actual = configuration.EntityEditors;

            var expected = new IEntityEditor[]
            {
                new NamespaceEditor(),
                new InterfaceEditor(),
                new PropertyAttributeEditor()
            };


            Assert.Equal(expected.Length, actual.Count);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i].GetType(), actual[i].GetType());
            }
        }
    }
}