using System.Linq;
using EntityScaffolding.Editors;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace EntityScaffolding
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "When using this API, and adding this class to the DI this error will show.")]
    public class ConventionEntityTypeGenerator : CSharpEntityTypeGenerator
    {
        private readonly IConventionConfiguration _configuration;

        public ConventionEntityTypeGenerator(ICSharpHelper cSharpHelper, IConventionConfiguration configuration = null) : base(cSharpHelper)
        {
            _configuration = configuration ?? new ConventionConfiguration();
        }

        public override string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            var entitySource = base.WriteCode(entityType, @namespace, useDataAnnotations);

            var allElements = _configuration.ConventionMatchers.FindApplicableConventions(entityType).ToList();

            return _configuration.EntityEditors.EditEntity(allElements, entityType, entitySource);
        }
    }
}