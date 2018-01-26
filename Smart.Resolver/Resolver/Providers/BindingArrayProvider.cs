namespace Smart.Resolver.Providers
{
    using System;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Helpers;

    /// <summary>
    ///
    /// </summary>
    internal sealed class BindingArrayProvider : IProvider
    {
        private readonly Func<int, Array> arrayAllocator;

        private readonly IBinding[] bindings;

        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="elementType"></param>
        /// <param name="components"></param>
        /// <param name="bindings"></param>
        public BindingArrayProvider(Type type, Type elementType, IComponentContainer components, IBinding[] bindings)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (elementType == null)
            {
                throw new ArgumentNullException(nameof(elementType));
            }

            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            if (bindings == null)
            {
                throw new ArgumentNullException(nameof(bindings));
            }

            TargetType = type;
            arrayAllocator = components.Get<IDelegateFactory>().CreateArrayAllocator(elementType);
            this.bindings = bindings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public Func<object> CreateFactory(IKernel kernel, IBinding binding)
        {
            var array = arrayAllocator(bindings.Length);
            var objs = (object[])array;
            for (var i = 0; i < bindings.Length; i++)
            {
                objs[i] = bindings[i].Provider.CreateFactory(kernel, bindings[i])();
            }

            return () => array;
        }
    }
}
