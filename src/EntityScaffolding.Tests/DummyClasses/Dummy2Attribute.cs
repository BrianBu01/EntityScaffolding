using System;

namespace EntityScaffolding.Tests.DummyClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Dummy2Attribute : Attribute
    {
        private readonly int _index;

        public Dummy2Attribute(int index)
        {
            _index = index;
        }

        public Dummy2Attribute()
        {
            _index = 0;
        }
    }
}