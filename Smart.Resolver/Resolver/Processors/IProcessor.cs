namespace Smart.Resolver.Processors
{
    using System;

    public interface IProcessor
    {
        int Order { get; }

        Action<IResolver, object> CreateProcessor(Type type);
    }
}
