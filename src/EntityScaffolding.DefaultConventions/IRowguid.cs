using System;

namespace EntityScaffolding.DefaultConventions
{
    [EntityConvention]
    public interface IRowguid
    {
        Guid Rowguid { get; set; }
    }
}