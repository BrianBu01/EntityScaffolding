using System;

namespace EntityScaffolding.DefaultConventions
{
    [EntityConvention]
    public interface IUpdateTracker
    {
        DateTime ModifiedDate { get; set; }
    }
}