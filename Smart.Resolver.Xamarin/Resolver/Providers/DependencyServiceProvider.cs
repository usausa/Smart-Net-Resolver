namespace Smart.Resolver.Providers
{
    using System;
    using System.Reflection;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;

    using Xamarin.Forms;

    /// <summary>
    ///
    /// </summary>
    public sealed class DependencyServiceProvider : IProvider
    {
        private sealed class DependencyServiceObjectFactory : IObjectFactory
        {
            private readonly MethodInfo method;

            public DependencyServiceObjectFactory(MethodInfo method)
            {
                this.method = method;
            }

            public object Create()
            {
                return method.Invoke(null, new object[] { DependencyFetchTarget.GlobalInstance });
            }
        }

        private readonly DependencyServiceObjectFactory objectFactory;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        public DependencyServiceProvider(Type type)
        {
            TargetType = type;
            var method = typeof(DependencyService).GetMethod("Get");
            objectFactory = new DependencyServiceObjectFactory(method.MakeGenericMethod(type));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public IProvider Copy(IComponentContainer components)
        {
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public IObjectFactory CreateFactory(IKernel kernel, IBinding binding)
        {
            return objectFactory;
        }
    }
}
