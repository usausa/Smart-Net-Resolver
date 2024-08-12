namespace Smart.Resolver;

#pragma warning disable SA1401
internal sealed class FactoryEntry
{
    public readonly bool CanGet;

    public readonly Func<IResolver, object> Single;

    public readonly Func<IResolver, object>[] Multiple;

    public FactoryEntry(bool canGet, Func<IResolver, object> single, Func<IResolver, object>[] multiple)
    {
        CanGet = canGet;
        Single = single;
        Multiple = multiple;
    }
}
#pragma warning restore SA1401
