using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.Editors;
using EntityScaffolding.Elements;
using EntityScaffolding.Tests.DummyClasses;
using EntityScaffolding.Tests.Mocking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Xunit;

namespace EntityScaffolding.Tests
{

    public class InterfaceEditorUnitTests
    {
        [Fact]
        public void AddSingleInterfaceNotFullyQualifiedTest()
        {
            var editor = new InterfaceEditor
            {
                TypeNameWriter = new TypeNameWriter(false),
                EntityType = MockUtilities.CreateEntityMock().Object,
                AllElements = new List<IWritableElement>
                {
                    new InterfaceElement(typeof(IAttributeTaggedEntityConventionDateTimeProperty))
                }
            };

            var edited = editor.EditEntity(@"using System;

public partial class Entity
{
}");

            var expected = @"using System;

public partial class Entity : IAttributeTaggedEntityConventionDateTimeProperty
{
}";

            Assert.Equal(expected, edited);
        }

        [Fact]
        public void AddMultipleInterfaceNotFullyQualifiedTest()
        {
            var editor = new InterfaceEditor
            {
                TypeNameWriter = new TypeNameWriter(false),
                EntityType = MockUtilities.CreateEntityMock().Object,
                AllElements = new List<IWritableElement>
                {
                    new InterfaceElement(typeof(IAttributeTaggedEntityConventionDateTimeProperty)),
                    new InterfaceElement(typeof(IAttributeTaggedEntityConventionNullableIntProperty))
                }
            };

            var edited = editor.EditEntity(@"using System;

public partial class Entity
{
}");

            var expected = @"using System;

public partial class Entity : IAttributeTaggedEntityConventionDateTimeProperty, IAttributeTaggedEntityConventionNullableIntProperty
{
}";

            Assert.Equal(expected, edited);
        }

        [Fact]
        public void AddNothingTest()
        {
            var editor = new InterfaceEditor
            {
                TypeNameWriter = new TypeNameWriter(false),
                EntityType = MockUtilities.CreateEntityMock().Object,
                AllElements = new List<IWritableElement>
                {
                    new PropertyAttributeElement()
                }
            };

            var edited = editor.EditEntity(@"using System;

public partial class Entity
{
}");

            var expected = @"using System;

public partial class Entity
{
}";

            Assert.Equal(expected, edited);
        }
    }
}
