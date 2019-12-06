using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EntityScaffolding.DefaultConventions.Tests
{
    public class DbSetExtensionsUnitTests
    {
        [Fact]
        public void FindEntity1GenericTest()
        {
            var mock = new Mock<DbSet<IIdentity<int>>>();

            mock.Setup(x => x.Find(Array.Empty<object>()));

            mock.Object.FindEntity(1101);

            mock.Verify(x => x.Find(1101), Times.Once);
        }

        [Fact]
        public void FindEntity2GenericTest()
        {
            var mock = new Mock<DbSet<IIdentity<int, bool>>>();

            mock.Setup(x => x.Find(Array.Empty<object>()));

            mock.Object.FindEntity(1101, false);

            mock.Verify(x => x.Find(1101, false), Times.Once);
        }

        [Fact]
        public void FindEntity3GenericTest()
        {
            var mock = new Mock<DbSet<IIdentity<int, bool, string>>>();

            mock.Setup(x => x.Find(Array.Empty<object>()));

            mock.Object.FindEntity(1101, false, "Hello World");

            mock.Verify(x => x.Find(1101, false, "Hello World"), Times.Once);
        }

        [Fact]
        public void FindEntity4GenericTest()
        {
            var mock = new Mock<DbSet<IIdentity<int, bool, string, uint?>>>();

            mock.Setup(x => x.Find(Array.Empty<object>()));

            mock.Object.FindEntity(1101, false, "Hello World", (uint?)null);

            mock.Verify(x => x.Find(1101, false, "Hello World", null), Times.Once);
        }

        [Fact]
        public void FindEntity5GenericTest()
        {
            var mock = new Mock<DbSet<IIdentity<int, bool, string, uint?, double>>>();

            mock.Setup(x => x.Find(Array.Empty<object>()));

            mock.Object.FindEntity(1101, false, "Hello World", (uint?)null, 1.3);

            mock.Verify(x => x.Find(1101, false, "Hello World", null, 1.3), Times.Once);
        }
    }
}