namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;

    public interface IInjector
    {
        // TODO 分離するか、CreateでTypeも見るか？
        //bool IsTarget(Type type);

        //Action<object> CreateInjector(IKernel kernel, IBinding binding);

        Action<object> CreateInjector(Type type, IKernel kernel, IBinding binding);
    }
}
