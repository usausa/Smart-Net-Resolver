namespace Smart.Resolver.Builders
{
    using System;
    using System.Linq;
    using System.Reflection;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Ignore")]
    public sealed class ReflectionFactoryBuilder : IFactoryBuilder
    {
        public object CreateFactory(ConstructorInfo ci, object[] factories, object[] actions)
        {
            Type factoryType;
            object instance;
            if (ci.GetParameters().Length == 0)
            {
                if (actions.Length == 0)
                {
                    factoryType = typeof(Factory<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType);
                }
                else
                {
                    factoryType = typeof(FactoryWithActions<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, new object[] { actions });
                }
            }
            else
            {
                if (actions.Length == 0)
                {
                    factoryType = typeof(FactoryWithArguments<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, ci, factories);
                }
                else
                {
                    factoryType = typeof(FactoryWithArgumentsAndActions<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, ci, factories, actions);
                }
            }

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IContainer), ci.DeclaringType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
        }

        private interface IFactory<out T>
        {
            T Create(IContainer container);
        }

        private sealed class Factory<T> : IFactory<T>
        {
            private static readonly Type Type = typeof(T);

            public T Create(IContainer container)
            {
                return (T)Activator.CreateInstance(Type);
            }
        }

        private sealed class FactoryWithActions<T> : IFactory<T>
        {
            private static readonly Type Type = typeof(T);

            private readonly Action<IContainer, T>[] actions;

            public FactoryWithActions(object[] actions)
            {
                this.actions = actions.Cast<Action<IContainer, T>>().ToArray();
            }

            public T Create(IContainer container)
            {
                var obj = (T)Activator.CreateInstance(Type);
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](container, obj);
                }

                return obj;
            }
        }

        private sealed class FactoryWithArguments<T> : IFactory<T>
        {
            private readonly ConstructorInfo ci;

            private readonly Func<IContainer, object>[] factories;

            public FactoryWithArguments(ConstructorInfo ci, object[] factories)
            {
                this.ci = ci;
                this.factories = factories.Cast<Func<IContainer, object>>().ToArray();
            }

            public T Create(IContainer container)
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](container);
                }

                return (T)ci.Invoke(args);
            }
        }

        private sealed class FactoryWithArgumentsAndActions<T> : IFactory<T>
        {
            private readonly ConstructorInfo ci;

            private readonly Func<IContainer, object>[] factories;

            private readonly Action<IContainer, T>[] actions;

            public FactoryWithArgumentsAndActions(ConstructorInfo ci, object[] factories, object[] actions)
            {
                this.ci = ci;
                this.factories = factories.Cast<Func<IContainer, object>>().ToArray();
                this.actions = actions.Cast<Action<IContainer, T>>().ToArray();
            }

            public T Create(IContainer container)
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](container);
                }

                var obj = (T)ci.Invoke(args);
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](container, obj);
                }

                return obj;
            }
        }

        public object CreateArrayFactory(Type type, object[] factories)
        {
            var arrayType = type.MakeArrayType();

            var factoryType = typeof(ArrayFactory<>).MakeGenericType(type);
            var instance = Activator.CreateInstance(factoryType, new object[] { factories });

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IContainer), arrayType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
        }

        private sealed class ArrayFactory<T> : IFactory<T[]>
        {
            private readonly Func<IContainer, T>[] factories;

            public ArrayFactory(object[] factories)
            {
                this.factories = factories.Cast<Func<IContainer, T>>().ToArray();
            }

            public T[] Create(IContainer container)
            {
                var array = new T[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    array[i] = factories[i](container);
                }

                return array;
            }
        }
    }
}
