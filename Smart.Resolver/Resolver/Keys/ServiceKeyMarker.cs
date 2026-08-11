namespace Smart.Resolver.Keys;

public sealed class ServiceKeyMarker
{
    public static readonly ServiceKeyMarker Instance = new();

    private ServiceKeyMarker()
    {
    }
}
