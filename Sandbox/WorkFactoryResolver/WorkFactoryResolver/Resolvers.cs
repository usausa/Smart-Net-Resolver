namespace WorkFactoryResolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Reflection;

    public interface IResolver
    {
        void RegisterSingleton(Type type);

        void RegisterTransient(Type type);

        object Get(Type type);
    }

    //--------------------------------------------------------------------------------
    // ObjectResolver
    //--------------------------------------------------------------------------------

    public interface IObjectProvider
    {
        object Create(IResolver resolver);
    }

    public static class ObjectProviderHelper
    {
        public static object Create(IResolver resolver, IActivator activator)
        {
            var parameters = activator.Source.GetParameters();
            var length = parameters.Length;
            if (length == 0)
            {
                return activator.Create();
            }

            var args = new object[length];
            for (var i = 0; i < length; i++)
            {
                args[i] = resolver.Get(parameters[i].ParameterType);
            }

            return activator.Create(args);
        }
    }

    public sealed class SingletonObjectProvider : IObjectProvider
    {
        private readonly IActivator activator;

        private object obj;

        public SingletonObjectProvider(Type type)
        {
            activator = TypeMetadataFactory.Default.CreateActivator(type.GetConstructors().First());
        }

        public object Create(IResolver resolver)
        {
            return obj ?? (obj = ObjectProviderHelper.Create(resolver, activator));
        }
    }

    public sealed class TransientObjectProvider : IObjectProvider
    {
        private readonly IActivator activator;

        public TransientObjectProvider(Type type)
        {
            activator = TypeMetadataFactory.Default.CreateActivator(type.GetConstructors().First());
        }

        public object Create(IResolver resolver)
        {
            return ObjectProviderHelper.Create(resolver, activator);
        }
    }

    public sealed class ObjectResolver : IResolver
    {
        private readonly Dictionary<Type, IObjectProvider> providers = new Dictionary<Type, IObjectProvider>();

        public void RegisterSingleton(Type type)
        {
            providers[type] = new SingletonObjectProvider(type);
        }

        public void RegisterTransient(Type type)
        {
            providers[type] = new TransientObjectProvider(type);
        }

        public object Get(Type type)
        {
            return providers.TryGetValue(type, out var provider) ? provider.Create(this) : null;
        }
    }

    //--------------------------------------------------------------------------------
    // FactoryResolver
    //--------------------------------------------------------------------------------

    public interface IObjectFactory
    {
        object Create();
    }

    public interface IFactoryProvider
    {
        IObjectFactory Resolve(IResolver resolver);
    }

    public sealed class FactoryResolver : IResolver
    {
        public void RegisterSingleton(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterTransient(Type type)
        {
            throw new NotImplementedException();
        }

        public object Get(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
