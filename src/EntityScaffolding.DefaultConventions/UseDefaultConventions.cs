namespace EntityScaffolding.DefaultConventions
{
    /// <summary>
    /// By adding this class to the service collection in the ConfigureDesignTimeServices method
    /// you will enable use of the Default Conventions during Scaffolding.
    /// </summary>
    /// <remarks>
    /// What is actually happening is, the DLL is being loaded into the the Domain and
    /// the EntityScaffolding will pick up from the current domain.
    /// </remarks>
    public class UseDefaultConventions
    {
        
    }
}
