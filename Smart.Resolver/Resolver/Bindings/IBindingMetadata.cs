namespace Smart.Resolver.Bindings
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Ignore")]
    public interface IBindingMetadata
    {
        string Name { get; }

        bool Has(string key);

        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);
    }
}
