namespace Smart.Resolver.Scopes
{
    using Smart.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    public interface IScope
    {
        IScopeStorage GetStorage(IResolver resolver, IComponentContainer components);
    }
}
