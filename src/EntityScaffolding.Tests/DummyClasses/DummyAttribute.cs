using System;
using System.Collections.Generic;
using System.Text;

namespace EntityScaffolding.Tests.DummyClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DummyAttribute : Attribute
    {
        private readonly int _index;

        public DummyAttribute(int index)
        {
            _index = index;
        }

        public DummyAttribute()
        {
            _index = 0;
        }
    }
}
