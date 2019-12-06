namespace EntityScaffolding.DefaultConventions
{
    [EntityConvention]
    public interface ISoftDelete
    {
        bool? IsDeleted { get; set; }
    }
}