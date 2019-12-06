using System.Collections.Generic;
using System.Linq;
using EntityScaffolding.Elements;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityScaffolding.Editors
{
    public abstract class EntityEditor<TWritableElements> : IEntityEditor
        where TWritableElements : IWritableElement
    {
        private List<TWritableElements> _allData;
        private ITypeNameWriter _typeNameWriter;

        public IEntityType EntityType { get; set; }

        public List<IWritableElement> AllElements { get; set; }

        public ITypeNameWriter TypeNameWriter
        {
            get => _typeNameWriter = _typeNameWriter ?? new TypeNameWriter(true);
            set => _typeNameWriter = value;
        }

        protected List<TWritableElements> WritableElements
        {
            get { return _allData = _allData ?? AllElements.OfType<TWritableElements>().ToList(); }
        }

        public string EditEntity(string entitySource)
        {
            return EditEntityBase(entitySource);
        }

        protected abstract string EditEntityBase(string entitySource);
    }
}