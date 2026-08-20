namespace Smart.Resolver.Scopes;

using Smart.ComponentModel;

public interface IScope
{
    bool TransferDisposal();

    IScope Copy(ComponentContainer components);

    Func<IResolver, object> Create(IResolver resolver, Func<IResolver, object> factory);
}
