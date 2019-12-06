using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.DefaultConventions.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Xunit;

namespace EntityScaffolding.DefaultConventions.Tests
{
    public class PrimaryKeyPropertyConventionMatcherUnitTests
    {
        [Fact]
        public void SingleKeyAttributeAppliedTest()
        {
            var matcher = new PrimaryKeyPropertyConventionMatcher();

            var property = new Mock<IProperty>();

            var key = new Mock<IKey>();
            key.Setup(x => x.Properties).Returns(new[] {property.Object});

            var entity = new Mock<IEntityType>();
            entity.Setup(x => x.FindPrimaryKey()).Returns(key.Object);


            var results = matcher.GetMatchingElements(entity.Object).ToList();

            Assert.Single(results);
            var first = results[0];
            Assert.Equal(typeof(PrimaryKeyAttribute), first.Attribute);
            Assert.Equal(property.Object, first.Property);
            Assert.Empty(first.AttributeValues);
        }

        [Fact]
        public void MultipleKeyAttributesAppliedTest()
        {
            var matcher = new PrimaryKeyPropertyConventionMatcher();

            var property1 = new Mock<IProperty>();
            var property2 = new Mock<IProperty>();
            var key = new Mock<IKey>();
            key.Setup(x => x.Properties).Returns(new[] {property1.Object, property2.Object});

            var entity = new Mock<IEntityType>();
            entity.Setup(x => x.FindPrimaryKey()).Returns(key.Object);


            var results = matcher.GetMatchingElements(entity.Object).ToList();

            Assert.Equal(2, results.Count);

            var first = results[0];
            Assert.Equal(typeof(PrimaryKeyAttribute), first.Attribute);
            Assert.Equal(property1.Object, first.Property);
            Assert.Equal(new List<string> {"0"}, first.AttributeValues);

            var second = results[1];
            Assert.Equal(typeof(PrimaryKeyAttribute), second.Attribute);
            Assert.Equal(property2.Object, second.Property);
            Assert.Equal(new List<string> {"1"}, second.AttributeValues);
        }
    }
}
