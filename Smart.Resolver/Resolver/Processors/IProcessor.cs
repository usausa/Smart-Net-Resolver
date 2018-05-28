namespace Smart.Resolver.Processors
{
    using System;

    public interface IProcessor
    {
        Action<object> CreateProcessor(Type type, IKernel kernel);
    }
}
