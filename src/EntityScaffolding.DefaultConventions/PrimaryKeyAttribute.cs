using System;

namespace EntityScaffolding.DefaultConventions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        public int Index { get; set; }

        public PrimaryKeyAttribute(int index = 0)
        {
            Index = index;
        }

    }
}