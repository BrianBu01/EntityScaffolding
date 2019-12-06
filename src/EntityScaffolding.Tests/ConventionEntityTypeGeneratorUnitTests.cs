using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.Elements;
using EntityScaffolding.Tests.DummyClasses;
using EntityScaffolding.Tests.Mocking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Xunit;

namespace EntityScaffolding.Tests
{
    //More of Integration Tests
    public class ConventionEntityTypeGeneratorUnitTests
    {
        [Fact]
        public void BaseLineWriterTest()
        {
            var helper = new Mock<ICSharpHelper>();
            var entityType = new Mock<IEntityType>();

            var generator = new ConventionEntityTypeGeneratorMock(helper.Object,
@"public partial class Entity
    {
        public string Property { get; set; }
    }");

            var entity = generator.WriteCode(entityType.Object, "DummyNameSpace", false);

            var expected = @"using System;
using System.Collections.Generic;

namespace DummyNameSpace
{
    public partial class Entity
    {
        public string Property { get; set; }
    }
}
";

            Assert.Equal(expected, entity);
        }

        [Fact]
        public void ApplyingInterfaceWriterTest()
        {
            var helper = new Mock<ICSharpHelper>();
            var entityType = MockUtilities.CreateEntityMock(typeof(string), "CustomProperty");

            var generator = new ConventionEntityTypeGeneratorMock(helper.Object,
                @"public partial class Entity
    {
        public string CustomProperty { get; set; }
    }");

            var entity = generator.WriteCode(entityType.Object, "DummyNameSpace", false);

            var expected = @"using System;
using System.Collections.Generic;
using EntityScaffolding.Tests.DummyClasses;

namespace DummyNameSpace
{
    public partial class Entity : IAttributeTaggedEntityConventionStringProperty
    {
        public string CustomProperty { get; set; }
    }
}
";

            Assert.Equal(expected, entity);
        }

        [Fact]
        public void ApplyingInterfaceWriterSameNamespaceTest()
        {
            var helper = new Mock<ICSharpHelper>();
            var entityType = MockUtilities.CreateEntityMock(typeof(string), "CustomProperty");

            var generator = new ConventionEntityTypeGeneratorMock(helper.Object,
                @"public partial class Entity
    {
        public string CustomProperty { get; set; }
    }");

            var entity = generator.WriteCode(entityType.Object, "EntityScaffolding.Tests.DummyClasses.Models", false);

            var expected = @"using System;
using System.Collections.Generic;

namespace EntityScaffolding.Tests.DummyClasses.Models
{
    public partial class Entity : IAttributeTaggedEntityConventionStringProperty
    {
        public string CustomProperty { get; set; }
    }
}
";

            Assert.Equal(expected, entity);
        }

        [Fact]
        public void ApplyingPropertyAttributeWriterTest()
        {
            var helper = new Mock<ICSharpHelper>();
            var entityType = MockUtilities.CreateEntityMock(typeof(string), "ApplyHere");
            MockEntityPropertyAttributeConventionMatcher.AppliesToEntityFunction = e =>
                e == entityType.Object ? new PropertyAttributeElement
                {
                    Attribute = typeof(DummyAttribute),
                    Property = entityType.Object.GetProperties().Single()
                } : null;

            var generator = new ConventionEntityTypeGeneratorMock(helper.Object,
                @"public partial class Entity
    {
        public string ApplyHere { get; set; }
    }");

            var entity = generator.WriteCode(entityType.Object, "EntityScaffolding.Tests.DummyClasses.Models", false);

            var expected = @"using System;
using System.Collections.Generic;

namespace EntityScaffolding.Tests.DummyClasses.Models
{
    public partial class Entity
    {
        
        [Dummy]
        public string ApplyHere { get; set; }
    }
}
";

            Assert.Equal(expected, entity);
        }

        [Fact]
        public void ApplyingPropertyAttributeParameterWriterTest()
        {
            var helper = new Mock<ICSharpHelper>();
            var entityType = MockUtilities.CreateEntityMock(typeof(string), "ApplyHere");
            MockEntityPropertyAttributeConventionMatcher.AppliesToEntityFunction = e =>
                e == entityType.Object ? new PropertyAttributeElement
                {
                    Attribute = typeof(DummyAttribute),
                    Property = entityType.Object.GetProperties().Single(),
                    AttributeValues = new List<string> { "1101" }
                } : null;

            var generator = new ConventionEntityTypeGeneratorMock(helper.Object,
                @"public partial class Entity
    {
        public string ApplyHere { get; set; }
    }");

            var entity = generator.WriteCode(entityType.Object, "EntityScaffolding.Tests.DummyClasses.Models", false);

            var expected = @"using System;
using System.Collections.Generic;

namespace EntityScaffolding.Tests.DummyClasses.Models
{
    public partial class Entity
    {
        
        [Dummy(1101)]
        public string ApplyHere { get; set; }
    }
}
";

            Assert.Equal(expected, entity);
        }
    }
}