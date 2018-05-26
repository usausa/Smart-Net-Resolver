namespace Smart.Resolver.Processors
{
    using System;

    public interface IProcessor
    {
        bool IsTarget(Type type);

        void Initialize(object instance);
    }
}
