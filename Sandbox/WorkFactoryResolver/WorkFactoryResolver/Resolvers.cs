namespace WorkFactoryResolver
{
    using System;
    using Smart.Collections.Concurrent;
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
        private readonly ThreadsafeTypeHashArrayMap<IObjectProvider> providers = new ThreadsafeTypeHashArrayMap<IObjectProvider>();

        public void RegisterSingleton(Type type)
        {
            providers.AddIfNotExist(type, new SingletonObjectProvider(type));
        }

        public void RegisterTransient(Type type)
        {
            providers.AddIfNotExist(type, new TransientObjectProvider(type));
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

    public sealed class ValueObjectFactory : IObjectFactory
    {
        private readonly object vaule;

        public ValueObjectFactory(object vaule)
        {
            this.vaule = vaule;
        }

        public object Create()
        {
            return vaule;
        }
    }

    public sealed class Parameter0ObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        public Parameter0ObjectFactory(IActivator activator)
        {
            this.activator = activator;
        }

        public object Create()
        {
            return activator.Create();
        }
    }

    public sealed class Parameter6ObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        public Parameter6ObjectFactory(
            IActivator activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
            this.argumentFactory6 = argumentFactory6;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create());
        }
    }

    public static class FactoryProviderHelper
    {
        public static IObjectFactory Create(FactoryResolver resolver, IActivator activator)
        {
            var parameters = activator.Source.GetParameters();
            var length = parameters.Length;

            if (length == 0)
            {
                return new Parameter0ObjectFactory(activator);
            }

            if (length == 6)
            {
                return new Parameter6ObjectFactory(
                    activator,
                    resolver.GetFactory(parameters[0].ParameterType),
                    resolver.GetFactory(parameters[1].ParameterType),
                    resolver.GetFactory(parameters[2].ParameterType),
                    resolver.GetFactory(parameters[3].ParameterType),
                    resolver.GetFactory(parameters[4].ParameterType),
                    resolver.GetFactory(parameters[5].ParameterType));
            }

            throw new NotImplementedException();
        }
    }

    public interface IFactoryProvider
    {
        IObjectFactory Resolve(FactoryResolver resolver);
    }

    public sealed class SingletonFactoryProvider : IFactoryProvider
    {
        private readonly IActivator activator;

        private IObjectFactory factory;

        public SingletonFactoryProvider(Type type)
        {
            activator = TypeMetadataFactory.Default.CreateActivator(type.GetConstructors().First());
        }

        public IObjectFactory Resolve(FactoryResolver resolver)
        {
            if (factory == null)
            {
                var provider = FactoryProviderHelper.Create(resolver, activator);
                if (provider != null)
                {
                    factory = new ValueObjectFactory(provider.Create());
                }
            }

            return factory;
        }
    }

    public sealed class TransientFactoryProvider : IFactoryProvider
    {
        private readonly IActivator activator;

        public TransientFactoryProvider(Type type)
        {
            activator = TypeMetadataFactory.Default.CreateActivator(type.GetConstructors().First());
        }

        public IObjectFactory Resolve(FactoryResolver resolver)
        {
            return FactoryProviderHelper.Create(resolver, activator);
        }
    }

    public sealed class FactoryResolver : IResolver
    {
        private readonly ThreadsafeTypeHashArrayMap<IFactoryProvider> providers = new ThreadsafeTypeHashArrayMap<IFactoryProvider>();

        private readonly ThreadsafeTypeHashArrayMap<IObjectFactory> factories = new ThreadsafeTypeHashArrayMap<IObjectFactory>();

        public void RegisterSingleton(Type type)
        {
            providers.AddIfNotExist(type, new SingletonFactoryProvider(type));
        }

        public void RegisterTransient(Type type)
        {
            providers.AddIfNotExist(type, new TransientFactoryProvider(type));
        }

        public object Get(Type type)
        {
            return GetFactory(type)?.Create();
        }

        public IObjectFactory GetFactory(Type type)
        {
            if (!factories.TryGetValue(type, out var factory))
            {
                if (!providers.TryGetValue(type, out var provider))
                {
                    return null;
                }

                factory = provider.Resolve(this);
                factories.AddIfNotExist(type, factory);
            }

            return factory;
        }
    }
}
