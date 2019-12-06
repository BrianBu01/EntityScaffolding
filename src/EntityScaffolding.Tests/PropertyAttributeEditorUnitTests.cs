using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.Editors;
using EntityScaffolding.Elements;
using EntityScaffolding.Tests.DummyClasses;
using EntityScaffolding.Tests.Mocking;
using Xunit;

namespace EntityScaffolding.Tests
{
    public class PropertyAttributeEditorUnitTests
    {
        [Fact]
        public void AddSingleInterfaceNotFullyQualifiedTest()
        {
            var entityType = MockUtilities.CreateEntityMock(typeof(int?), "Value").Object;
            var editor = new PropertyAttributeEditor
            {
                TypeNameWriter = new TypeNameWriter(false),
                EntityType = entityType,
                AllElements = new List<IWritableElement>
                {
                    new PropertyAttributeElement
                    {
                        Attribute = typeof(DummyAttribute),
                        AttributeValues = new List<string>{ "1001" },
                        Property = entityType.GetProperties().Single()
                    }
                }
            };

            var edited = editor.EditEntity(@"using System;

public partial class Entity
{
        public int? Value { get; set; }
}");

            var expected = @"using System;

public partial class Entity
{
        
        [Dummy(1001)]
        public int? Value { get; set; }
}";

            Assert.Equal(expected, edited);
        }

        [Fact]
        public void AddMultipleInterfaceNotFullyQualifiedTest()
        {
            var entityType = MockUtilities.CreateEntityMock(typeof(int), "Value").Object;
            var editor = new PropertyAttributeEditor
            {
                TypeNameWriter = new TypeNameWriter(false),
                EntityType = entityType,
                AllElements = new List<IWritableElement>
                {
                    new PropertyAttributeElement
                    {
                        Attribute = typeof(DummyAttribute),
                        AttributeValues = new List<string>{ "1001", "\"string\"" },
                        Property = entityType.GetProperties().Single()
                    },
                    new PropertyAttributeElement
                    {
                        Attribute = typeof(Dummy2Attribute),
                        Property = entityType.GetProperties().Single()
                    }
                }
            };

            var edited = editor.EditEntity(@"using System;

public partial class Entity
{
        public int Value { get; set; }
}");

            var expected = @"using System;

public partial class Entity
{
        
        [Dummy(1001, ""string"")]
        [Dummy2]
        public int Value { get; set; }
}";
            Assert.Equal(expected, edited);
        }

        [Fact]
        public void AddNothingTest()
        {
            var entityType = MockUtilities.CreateEntityMock(typeof(int), "Value").Object;
            var editor = new PropertyAttributeEditor
            {
                TypeNameWriter = new TypeNameWriter(false),
                EntityType = entityType,
                AllElements = new List<IWritableElement>
                {
                    new InterfaceElement(typeof(IAttributeTaggedEntityConventionDateTimeProperty))
                }
            };

            var edited = editor.EditEntity(@"using System;

public partial class Entity
{
        public int Value { get; set; }
}");

            var expected = @"using System;

public partial class Entity
{
        public int Value { get; set; }
}";
            Assert.Equal(expected, edited);

        }
    }
}