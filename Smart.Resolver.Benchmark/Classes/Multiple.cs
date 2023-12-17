namespace Smart.Resolver.Benchmark.Classes;

public interface IMultipleSingletonService
{
    void DoSomething();
}

public sealed class MultipleSingletonService1 : IMultipleSingletonService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleSingletonService2 : IMultipleSingletonService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleSingletonService3 : IMultipleSingletonService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleSingletonService4 : IMultipleSingletonService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleSingletonService5 : IMultipleSingletonService
{
    public void DoSomething()
    {
    }
}

public interface IMultipleTransientService
{
    void DoSomething();
}

public sealed class MultipleTransientService1 : IMultipleTransientService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleTransientService2 : IMultipleTransientService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleTransientService3 : IMultipleTransientService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleTransientService4 : IMultipleTransientService
{
    public void DoSomething()
    {
    }
}

public sealed class MultipleTransientService5 : IMultipleTransientService
{
    public void DoSomething()
    {
    }
}
