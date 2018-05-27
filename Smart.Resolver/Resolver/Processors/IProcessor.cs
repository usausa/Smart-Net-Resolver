namespace Smart.Resolver.Processors
{
    using System;

    public interface IProcessor
    {
        bool IsTarget(Type type);

        Action<object> CreateProcessor(IKernel kernel);
    }
}
