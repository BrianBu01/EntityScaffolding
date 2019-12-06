using System;
using System.Collections.Generic;
using EntityScaffolding.Editors;
using EntityScaffolding.Tests.DummyClasses;
using EntityScaffolding.Tests.DummyClasses.More;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Xunit;

namespace EntityScaffolding.Tests
{

    public class TypeNameWriterUnitTests
    {
        [Theory]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(decimal), "decimal")]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(char), "char")]
        [InlineData(typeof(sbyte), "sbyte")]
        [InlineData(typeof(ushort), "ushort")]
        [InlineData(typeof(uint), "uint")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(ulong), "ulong")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(DateTime), "DateTime")]
        [InlineData(typeof(string), "string")]

        public void TypeNameShortedPrimitivesTest(Type type, string expected)
        {
            var writer = new TypeNameWriter(false);

            var actual = writer.GetTypeName(type);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(int), "int")]
        [InlineData(typeof(bool), "bool")]
        [InlineData(typeof(double), "double")]
        [InlineData(typeof(short), "short")]
        [InlineData(typeof(decimal), "decimal")]
        [InlineData(typeof(byte), "byte")]
        [InlineData(typeof(object), "object")]
        [InlineData(typeof(char), "char")]
        [InlineData(typeof(sbyte), "sbyte")]
        [InlineData(typeof(ushort), "ushort")]
        [InlineData(typeof(uint), "uint")]
        [InlineData(typeof(long), "long")]
        [InlineData(typeof(ulong), "ulong")]
        [InlineData(typeof(float), "float")]
        [InlineData(typeof(DateTime), "DateTime")]
        [InlineData(typeof(string), "string")]

        public void TypeNameQualifiedPrimitivesTest(Type type, string expected)
        {
            var writer = new TypeNameWriter(true);

            var actual = writer.GetTypeName(type);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(DummyAttribute), "EntityScaffolding.Tests.DummyClasses.DummyAttribute")]
        [InlineData(typeof(IAnotherInterface), "EntityScaffolding.Tests.DummyClasses.More.IAnotherInterface")]

        public void TypeNameQualifiedTest(Type type, string expected)
        {
            var writer = new TypeNameWriter(true);

            var actual = writer.GetTypeName(type);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(DummyAttribute), "DummyAttribute")]
        [InlineData(typeof(IAnotherInterface), "IAnotherInterface")]

        public void TypeNameShortedTest(Type type, string expected)
        {
            var writer = new TypeNameWriter(false);

            var actual = writer.GetTypeName(type);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(IGenericType<int>), "EntityScaffolding.Tests.DummyClasses.IGenericType<int>")]
        [InlineData(typeof(IGenericType<int, IAnotherInterface>), "EntityScaffolding.Tests.DummyClasses.IGenericType<int, EntityScaffolding.Tests.DummyClasses.More.IAnotherInterface>")]
        [InlineData(typeof(int?), "int?")]
        [InlineData(typeof(DummyStruct?), "EntityScaffolding.Tests.DummyClasses.DummyStruct?")]
        public void TypeNameGenericQualifiedTest(Type type, string expected)
        {
            var writer = new TypeNameWriter(true);

            var actual = writer.GetTypeName(type);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(IGenericType<int>), "IGenericType<int>")]
        [InlineData(typeof(IGenericType<int, IAnotherInterface>), "IGenericType<int, IAnotherInterface>")]
        [InlineData(typeof(int?), "int?")]
        [InlineData(typeof(DummyStruct?), "DummyStruct?")]

        public void TypeNameGenericShortedTest(Type type, string expected)
        {
            var writer = new TypeNameWriter(false);

            var actual = writer.GetTypeName(type);

            Assert.Equal(expected, actual);
        }
    }
}
