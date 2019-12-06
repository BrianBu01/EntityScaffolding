using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EntityScaffolding.Tests.Mocking
{
    public class ConventionEntityTypeGeneratorMock : ConventionEntityTypeGenerator
    {
        private readonly string _classBody;
        private IndentedStringBuilder _sbCached;

        private IndentedStringBuilder StringBuilder
        {
            get
            {
                return _sbCached = _sbCached ?? (IndentedStringBuilder)typeof(CSharpEntityTypeGenerator)
                    .GetField("_sb", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(this);
            }
        }

        public ConventionEntityTypeGeneratorMock(ICSharpHelper cSharpHelper, string classBody = null, IConventionConfiguration configuration = null) :
            base(cSharpHelper, configuration)
        {
            _classBody = classBody;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<Pending>")]
        protected override void GenerateClass(IEntityType entityType)
        {
            //Defaulting writing of class body instead of mocking out all the internal code.
            StringBuilder.AppendLine(_classBody ?? String.Empty);
        }
    }
}