using System;
using System.Linq;
using EntityScaffolding.DefaultConventions.Matchers;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Xunit;

namespace EntityScaffolding.DefaultConventions.Tests
{
    public class IdentityConventionMatcherUnitTests
    {
        [Fact]
        public void SingleKeyInterfaceAppliedTest()
        {
            var matcher = new IdentityConventionMatcher();

            var property = new Mock<IProperty>();
            property.Setup(x => x.ClrType).Returns(typeof(int?));

            var key = new Mock<IKey>();
            key.Setup(x => x.Properties).Returns(new[] { property.Object });

            var entity = new Mock<IEntityType>();
            entity.Setup(x => x.FindPrimaryKey()).Returns(key.Object);


            var results = matcher.GetMatchingElements(entity.Object).ToList();

            Assert.Single(results);
            Assert.Equal(typeof(IIdentity<int?>), results.Single().InterfaceType);
        }

        [Fact]
        public void MultipleKeyInterfaceAppliedTest()
        {
            var matcher = new IdentityConventionMatcher();

            var property1 = new Mock<IProperty>();
            property1.Setup(x => x.ClrType).Returns(typeof(int?));
            var property2 = new Mock<IProperty>();
            property2.Setup(x => x.ClrType).Returns(typeof(string));
            var key = new Mock<IKey>();
            key.Setup(x => x.Properties).Returns(new[] { property1.Object, property2.Object });

            var entity = new Mock<IEntityType>();
            entity.Setup(x => x.FindPrimaryKey()).Returns(key.Object);


            var results = matcher.GetMatchingElements(entity.Object).ToList();

            Assert.Single(results);
            Assert.Equal(typeof(IIdentity<int?, string>), results.Single().InterfaceType);

        }

        [Fact]
        public void NoKeyInterfaceAppliedTest()
        {
            var matcher = new IdentityConventionMatcher();

            var key = new Mock<IKey>();
            key.Setup(x => x.Properties).Returns(Array.Empty<IProperty>());

            var entity = new Mock<IEntityType>();
            entity.Setup(x => x.FindPrimaryKey()).Returns(key.Object);


            var results = matcher.GetMatchingElements(entity.Object).ToList();

            Assert.Empty(results);
        }
    }
}