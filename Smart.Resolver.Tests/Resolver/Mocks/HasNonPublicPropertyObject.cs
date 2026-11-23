namespace Smart.Resolver.Mocks;

using Smart.Resolver.Attributes;

public class NonPublicPropertyBase
{
    [Inject]
    protected SimpleObject? Protected { get; set; }

    public SimpleObject? ProtectedValue => Protected;
}

public sealed class HasNonPublicPropertyObject : NonPublicPropertyBase
{
    [Inject]
    public SimpleObject? PrivateSetter { get; private set; }

    [Inject]
    internal SimpleObject? Internal { get; set; }

    [Inject]
    private SimpleObject? Private { get; set; }

    public SimpleObject? PrivateValue => Private;
}
