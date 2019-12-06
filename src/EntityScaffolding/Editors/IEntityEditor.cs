using System.Collections.Generic;
using EntityScaffolding.Elements;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Editors
{
    public interface IEntityEditor
    {
        IEntityType EntityType { get; set; }

        List<IWritableElement> AllElements { get; set; }

        ITypeNameWriter TypeNameWriter { get; set; }

        string EditEntity(string entitySource);
    }
}
