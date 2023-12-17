namespace Example.WebApplication.Services;

public sealed class MetricsManager
{
    private int counter;

    public int Counter => counter;

    public void Increment()
    {
        Interlocked.Add(ref counter, 1);
    }
}
