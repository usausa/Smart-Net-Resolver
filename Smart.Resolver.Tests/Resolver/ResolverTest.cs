namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Providers;

    using Xunit;

    public class ResolverTest
    {
        [Fact]
        public void ObjectBindingCreatedBySelfMissingResolver()
        {
            var config = new ResolverConfig().UseAutoBinding();
            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SimpleObject>();

                Assert.NotNull(obj);
            }
        }

        [Fact]
        public void ObjectBindingCreatedByOpenGenericMissingResolver()
        {
            var config = new ResolverConfig().UseOpenGenericBinding();
            config
                .Bind(typeof(IGenericService<>))
                .To(typeof(GenericService<>));

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get(typeof(IGenericService<int>));

                Assert.NotNull(obj);
                Assert.Equal(typeof(GenericService<int>), obj.GetType());
            }
        }

        public interface IGenericService<out T>
        {
            T Create();
        }

        public class GenericService<T> : IGenericService<T>
        {
            public T Create()
            {
                return default;
            }
        }

        [Fact]
        public void ObjectBindingCreatedByAssignableMissingResolver()
        {
            var config = new ResolverConfig().UseAssignableBinding();
            config.Bind(typeof(ExecuteService)).ToSelf().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<ExecuteService>();
                var obj1 = resolver.Get<IExecuteService>();
                var obj2 = resolver.Get<ExecuteServiceBase>();

                Assert.NotNull(obj);
                Assert.NotNull(obj1);
                Assert.NotNull(obj2);
                Assert.Same(obj1, obj);
                Assert.Same(obj2, obj);
            }
        }

        public interface IExecuteService
        {
            void Execute();
        }

        public abstract class ExecuteServiceBase : IExecuteService
        {
            public void Execute()
            {
                OnExecute();
            }

            protected abstract void OnExecute();
        }

        public class ExecuteService : ExecuteServiceBase
        {
            protected override void OnExecute()
            {
            }
        }

        [Fact]
        public void ObjectBindingCreatedByArrayMissingResolver()
        {
            var config = new ResolverConfig().UseArrayBinding();
            config.Bind<IMultipleService>().To<MultipleService1>().InSingletonScope();
            config.Bind<IMultipleService>().To<MultipleService2>().InSingletonScope();
            config.Bind<IMultipleService>().To<MultipleService3>().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var objects = resolver.Get<IEnumerable<IMultipleService>>();

                Assert.NotNull(objects);
                Assert.Equal(3, objects.Count());
            }
        }

        public interface IMultipleService
        {
            void Execute();
        }

        public class MultipleService1 : IMultipleService
        {
            public void Execute()
            {
            }
        }

        public class MultipleService2 : IMultipleService
        {
            public void Execute()
            {
            }
        }

        public class MultipleService3 : IMultipleService
        {
            public void Execute()
            {
            }
        }

        [Fact]
        public void ObjectBindingCreatedByCustomBindingResolver()
        {
            var config = new ResolverConfig();
            var typeMap = new Dictionary<Type, Type>
            {
                [typeof(IService)] = typeof(Service)
            };
            config.UseMissingHandler(new CustomMissingHandler(typeMap));

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<IService>();

                Assert.NotNull(obj);
            }
        }

        public sealed class CustomMissingHandler : IMissingHandler
        {
            private readonly Dictionary<Type, Type> typeMap;

            public CustomMissingHandler(Dictionary<Type, Type> typeMap)
            {
                this.typeMap = typeMap;
            }

            public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
            {
                if (!typeMap.TryGetValue(type, out Type _))
                {
                    return Enumerable.Empty<IBinding>();
                }

                return new[]
                {
                    new Binding(type, new StandardProvider(typeof(Service), components))
                };
            }
        }
    }
}
