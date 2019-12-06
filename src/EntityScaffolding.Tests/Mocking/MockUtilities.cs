using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;

namespace EntityScaffolding.Tests.Mocking
{
    internal static class MockUtilities
    {
        public static Mock<IEntityType> CreateEntityMock(Type propertyType = null, string propertyName = null,
            string entityName = null)
        {
            var entityMock = new Mock<IEntityType>();

            if (propertyType != null && !string.IsNullOrEmpty(propertyName))
            {
                var propertyMock = new Mock<IProperty>();

                propertyMock.Setup(x => x.ClrType).Returns(propertyType);
                propertyMock.Setup(x => x.Name).Returns(propertyName);

                entityMock.Setup(x => x.GetProperties()).Returns(() =>
                    new List<IProperty>
                    {
                        propertyMock.Object
                    });
            }

            entityMock.Setup(x => x.Name).Returns(entityName ?? "Entity");
            
            return entityMock;
        }
    }
}