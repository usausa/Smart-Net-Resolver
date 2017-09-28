namespace Smart.Resolver.Providers
{
    using System;
    using System.Reflection;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    using Xamarin.Forms;

    /// <summary>
    ///
    /// </summary>
    public class DependencyServiceProvider : IProvider
    {
        private readonly MethodInfo genericMethod;

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
            var method = typeof(DependencyService).GetTypeInfo().GetDeclaredMethod("Get");
            genericMethod = method.MakeGenericMethod(type);
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
        public object Create(IKernel kernel, IBinding binding)
        {
            return genericMethod.Invoke(null, new object[] { DependencyFetchTarget.GlobalInstance });
        }
    }
}
