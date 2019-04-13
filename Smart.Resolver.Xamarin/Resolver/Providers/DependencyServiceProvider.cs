namespace Smart.Resolver.Providers
{
    using System;
    using System.Reflection;

    using Smart.Resolver.Bindings;

    using Xamarin.Forms;

    public sealed class DependencyServiceProvider : IProvider
    {
        public Type TargetType { get; }

        public DependencyServiceProvider(Type type)
        {
            TargetType = type;
        }

        public Func<IKernel, object> CreateFactory(IKernel kernel, IBinding binding)
        {
            var method = typeof(DependencyService).GetMethod("Get");
            var genericMethod = method.MakeGenericMethod(TargetType);
            return CreateFactory(genericMethod);
        }

        private static Func<IKernel, object> CreateFactory(MethodInfo method)
        {
            return k => method.Invoke(null, new object[] { DependencyFetchTarget.GlobalInstance });
        }
    }
}
