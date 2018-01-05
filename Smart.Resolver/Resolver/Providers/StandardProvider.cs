namespace Smart.Resolver.Providers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public sealed class StandardProvider : IProvider
    {
        private readonly IInjector[] injectors;

        private readonly IActivatorFactory activatorFactory;

        private readonly TypeMetadata metadata;

        private ConstructorMetadata constructor;

        private IActivator activator;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="components"></param>
        public StandardProvider(Type type, IComponentContainer components)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (components == null)
            {
                throw new ArgumentNullException(nameof(components));
            }

            TargetType = type;
            injectors = components.GetAll<IInjector>().ToArray();
            activatorFactory = components.Get<IActivatorFactory>();
            metadata = components.Get<IMetadataFactory>().GetMetadata(TargetType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public IProvider Copy(IComponentContainer components)
        {
            return new StandardProvider(TargetType, components);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IObjectFactory CreateFactory(IKernel kernel, IBinding binding)
        {
            // TODO
            return null;
        }

        //public IObjectFactory CreateFactory(IKernel kernel, IBinding binding)
        //{
        //    if (constructor == null)
        //    {
        //        Interlocked.CompareExchange(ref constructor, FindBestConstructor(kernel, binding), null);

        //        if (activator == null)
        //        {
        //            Interlocked.CompareExchange(ref activator, activatorFactory.CreateActivator(constructor.Constructor), null);
        //        }
        //    }

        //    var arguments = constructor.Parameters.Length == 0 ? null : ResolveParameters(kernel, binding, constructor);
        //    var instance = activator.Create(arguments);

        //    for (var j = 0; j < injectors.Length; j++)
        //    {
        //        injectors[j].Inject(kernel, binding, metadata, instance);
        //    }

        //    return instance;
        //}

        //private ConstructorMetadata FindBestConstructor(IKernel kernel, IBinding binding)
        //{
        //    if (metadata.TargetConstructors.Length == 0)
        //    {
        //        throw new InvalidOperationException(
        //            String.Format(CultureInfo.InvariantCulture, "No constructor avaiable. type = {0}", TargetType.Name));
        //    }

        //    for (var i = 0; i < metadata.TargetConstructors.Length; i++)
        //    {
        //        var match = true;
        //        var cm = metadata.TargetConstructors[i];

        //        var parameters = cm.Parameters;
        //        for (var j = 0; j < parameters.Length; j++)
        //        {
        //            var parameter = parameters[j];
        //            var pi = parameter.Parameter;

        //            // Constructor argument
        //            if (binding.ConstructorArguments.GetParameter(pi.Name) != null)
        //            {
        //                continue;
        //            }

        //            // Multiple
        //            if (parameter.ElementType != null)
        //            {
        //                continue;
        //            }

        //            // Resolve
        //            if (kernel.CanResolve(pi.ParameterType, cm.Constraints[j]))
        //            {
        //                continue;
        //            }

        //            // DefaultValue
        //            if (pi.HasDefaultValue)
        //            {
        //                continue;
        //            }

        //            match = false;
        //            break;
        //        }

        //        if (match)
        //        {
        //            return cm;
        //        }
        //    }

        //    throw new InvalidOperationException(
        //        String.Format(CultureInfo.InvariantCulture, "Constructor parameter unresolved. type = {0}", TargetType.Name));
        //}

        //private static object[] ResolveParameters(IKernel kernel, IBinding binding, ConstructorMetadata constructor)
        //{
        //    var parameters = constructor.Parameters;
        //    var arguments = new object[parameters.Length];
        //    for (var i = 0; i < arguments.Length; i++)
        //    {
        //        var parameter = parameters[i];
        //        var pi = parameter.Parameter;

        //        // Constructor argument
        //        var argument = binding.ConstructorArguments.GetParameter(pi.Name);
        //        if (argument != null)
        //        {
        //            arguments[i] = argument.Resolve(kernel);
        //            continue;
        //        }

        //        // Multiple
        //        if (parameter.ElementType != null)
        //        {
        //            arguments[i] = ResolverHelper.ConvertArray(parameter.ElementType, kernel.ResolveAll(parameter.ElementType, constructor.Constraints[i]));
        //            continue;
        //        }

        //        // Resolve
        //        var obj = kernel.TryResolve(pi.ParameterType, constructor.Constraints[i], out bool resolve);
        //        if (resolve)
        //        {
        //            arguments[i] = obj;
        //            continue;
        //        }

        //        // DefaultValue
        //        if (pi.HasDefaultValue)
        //        {
        //            arguments[i] = pi.DefaultValue;
        //        }
        //    }

        //    return arguments;
        //}
    }
}
