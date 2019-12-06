using System;
using System.Collections.Generic;

namespace EntityScaffolding.Elements
{
    public interface IWritableElement
    {
        IEnumerable<Type> UsedTypes { get; }
    }
}