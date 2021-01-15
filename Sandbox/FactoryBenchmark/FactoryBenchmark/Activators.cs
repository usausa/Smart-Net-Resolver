namespace FactoryBenchmark
{
    public interface IActivator0 : IActivator
    {
        object Create();
    }

    public interface IActivator1 : IActivator
    {
        object Create(object argument1);
    }

    public interface IActivator6 : IActivator
    {
        object Create(
            object argument1,
            object argument2,
            object argument3,
            object argument4,
            object argument5,
            object argument6);
    }

    public interface IActivator
    {
        object Create(params object[] arguments);
    }
}
