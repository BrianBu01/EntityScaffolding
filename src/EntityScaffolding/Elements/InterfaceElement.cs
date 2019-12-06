using System;
using System.Collections.Generic;

namespace EntityScaffolding.Elements
{
    public class InterfaceElement : IWritableElement
    {
        public InterfaceElement(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new InvalidOperationException($"Type {interfaceType.Name} is not an interface.");
            }
            InterfaceType = interfaceType;
        }

        public Type InterfaceType { get; }

        public IEnumerable<Type> UsedTypes => new[] { InterfaceType };
    }
}