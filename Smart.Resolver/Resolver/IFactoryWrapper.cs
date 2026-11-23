namespace Smart.Resolver;

internal interface IFactoryWrapper
{
    Func<IResolver, object> Factory { get; }
}
