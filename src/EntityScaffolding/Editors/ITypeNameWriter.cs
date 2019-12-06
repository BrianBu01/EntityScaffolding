using System;

namespace EntityScaffolding.Editors
{
    public interface ITypeNameWriter
    {
        bool RequiresFullyQualifiedNames { get; }

        string GetTypeName(Type type);
    }
}