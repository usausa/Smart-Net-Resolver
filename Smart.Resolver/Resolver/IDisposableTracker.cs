namespace Smart.Resolver;

internal interface IDisposableTracker
{
    void TrackDisposable(IDisposable disposable);
}
