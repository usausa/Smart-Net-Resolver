namespace Smart.Resolver.Scopes;

using Smart.ComponentModel;

public interface IScope
{
    IScope Copy(ComponentContainer components);

    Func<IResolver, object> Create(Func<object> factory);
}
