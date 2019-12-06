using System.Collections.Generic;
using EntityScaffolding.Editors;
using EntityScaffolding.Elements;
using EntityScaffolding.Tests.DummyClasses;
using EntityScaffolding.Tests.DummyClasses.More;
using EntityScaffolding.Tests.Mocking;
using Xunit;

namespace EntityScaffolding.Tests
{
    public class NamespaceEditorUnitTests
    {
        [Fact]
        public void AddSingleUsingStatement()
        {
            var entityType = MockUtilities.CreateEntityMock(typeof(int?), "Value").Object;
            var editor = new NamespaceEditor
            {
                EntityType = MockUtilities.CreateEntityMock().Object,
                AllElements = new List<IWritableElement>
                {
                    new PropertyAttributeElement
                    {
                        Attribute = typeof(DummyAttribute)
                    }
                }
            };

            var edited = editor.EditEntity(@"using System;

namespace EntityScaffolding.Tests.Models
{
    Class
}");

            var expected = $@"using System;
using {typeof(DummyAttribute).Namespace};

namespace EntityScaffolding.Tests.Models
{{
    Class
}}";

            Assert.Equal(expected, edited);
            Assert.False(editor.TypeNameWriter.RequiresFullyQualifiedNames);
        }

        [Fact]
        public void DoNotAddUsingWhenConflictingNamesUnitTest()
        {
            var entityType = MockUtilities.CreateEntityMock(typeof(int?), "Value").Object;
            var editor = new NamespaceEditor
            {
                EntityType = MockUtilities.CreateEntityMock().Object,
                AllElements = new List<IWritableElement>
                {
                    new InterfaceElement(typeof(EntityScaffolding.Tests.DummyClasses.MoreAgain.IConflictingName)),
                    new InterfaceElement(typeof(EntityScaffolding.Tests.DummyClasses.More.IConflictingName))
                }
            };

            var edited = editor.EditEntity(@"using System;

namespace EntityScaffolding.Tests.Models
{
    Class
}");

            var expected = $@"using System;

namespace EntityScaffolding.Tests.Models
{{
    Class
}}";

            Assert.Equal(expected, edited);
            Assert.True(editor.TypeNameWriter.RequiresFullyQualifiedNames);
        }

        [Fact]
        public void DoNotAddUsingWhenInNameSpaceUnitTest()
        {
            var entityType = MockUtilities.CreateEntityMock(typeof(int?), "Value").Object;
            var editor = new NamespaceEditor
            {
                EntityType = MockUtilities.CreateEntityMock().Object,
                AllElements = new List<IWritableElement>
                {
                    new InterfaceElement(typeof(IAttributeTaggedEntityConventionDateTimeProperty))
                }
            };

            var edited = editor.EditEntity(@"using System;

namespace EntityScaffolding.Tests.DummyClasses.Models
{
    Class
}");

            var expected = $@"using System;

namespace EntityScaffolding.Tests.DummyClasses.Models
{{
    Class
}}";

            Assert.Equal(expected, edited);
            Assert.False(editor.TypeNameWriter.RequiresFullyQualifiedNames);
        }
    }
}