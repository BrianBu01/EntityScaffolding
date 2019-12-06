using System.Collections.Generic;
using EntityScaffolding.Elements;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Editors
{
    public static class EntityEditorExtensions
    {
        public static string EditEntity(this IEnumerable<IEntityEditor> editors, List<IWritableElement> elements,
            IEntityType entityType, string entitySource, ITypeNameWriter typeNameWriter = null)
        {
            foreach (var editor in editors)
            {
                //The next editor get the previous editor's data.
                editor.TypeNameWriter = typeNameWriter;
                editor.AllElements = elements;
                editor.EntityType = entityType;

                entitySource = editor.EditEntity(entitySource);

                typeNameWriter = editor.TypeNameWriter;
                elements = editor.AllElements;
                entityType = editor.EntityType;
            }

            return entitySource;
        }
    }
}