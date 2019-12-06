using System;

namespace EntityScaffolding.DefaultConventions
{
    [EntityConvention]
    public interface IInsertTracker
    {
        DateTime CreatedDate { get; set; }
    }
}