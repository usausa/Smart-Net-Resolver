namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Ignore")]
    public sealed class ReflectionFactoryBuilder : IFactoryBuilder
    {
        public object CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
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
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), ci.DeclaringType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
        }

        private interface IFactory<out T>
        {
            T Create(IResolver resolver);
        }

        private sealed class Factory<T> : IFactory<T>
        {
            private static readonly Type Type = typeof(T);

            public T Create(IResolver resolver)
            {
                return (T)Activator.CreateInstance(Type);
            }
        }

        private sealed class FactoryWithActions<T> : IFactory<T>
        {
            private static readonly Type Type = typeof(T);

            private readonly Action<IResolver, object>[] actions;

            public FactoryWithActions(Action<IResolver, object>[] actions)
            {
                this.actions = actions;
            }

            public T Create(IResolver resolver)
            {
                var obj = (T)Activator.CreateInstance(Type);
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](resolver, obj);
                }

                return obj;
            }
        }

        private sealed class FactoryWithArguments<T> : IFactory<T>
        {
            private readonly ConstructorInfo ci;

            private readonly Func<IResolver, object>[] factories;

            public FactoryWithArguments(ConstructorInfo ci, Func<IResolver, object>[] factories)
            {
                this.ci = ci;
                this.factories = factories;
            }

            public T Create(IResolver resolver)
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](resolver);
                }

                return (T)ci.Invoke(args);
            }
        }

        private sealed class FactoryWithArgumentsAndActions<T> : IFactory<T>
        {
            private readonly ConstructorInfo ci;

            private readonly Func<IResolver, object>[] factories;

            private readonly Action<IResolver, object>[] actions;

            public FactoryWithArgumentsAndActions(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
            {
                this.ci = ci;
                this.factories = factories;
                this.actions = actions;
            }

            public T Create(IResolver resolver)
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](resolver);
                }

                var obj = (T)ci.Invoke(args);
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](resolver, obj);
                }

                return obj;
            }
        }

        public object CreateArrayFactory(Type type, Func<IResolver, object>[] factories)
        {
            var arrayType = type.MakeArrayType();

            var factoryType = typeof(ArrayFactory<>).MakeGenericType(type);
            var instance = Activator.CreateInstance(factoryType, new object[] { factories });

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), arrayType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
        }

        private sealed class ArrayFactory<T> : IFactory<T[]>
        {
            private readonly Func<IResolver, object>[] factories;

            public ArrayFactory(Func<IResolver, object>[] factories)
            {
                this.factories = factories;
            }

            public T[] Create(IResolver resolver)
            {
                var array = new T[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    array[i] = (T)factories[i](resolver);
                }

                return array;
            }
        }
    }
}
