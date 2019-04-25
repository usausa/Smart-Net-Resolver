namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Ignore")]
    public sealed class ReflectionFactoryBuilder : IFactoryBuilder
    {
        public Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
        {
            Type factoryType;
            object instance;
            if (ci.GetParameters().Length == 0)
            {
                if (actions.Length == 0)
                {
                    factoryType = ci.DeclaringType.IsValueType
                        ? typeof(ValueTypeFactory)
                        : typeof(Factory<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, ci.DeclaringType);
                }
                else
                {
                    factoryType = ci.DeclaringType.IsValueType
                        ? typeof(ValueTypeFactoryWithActions)
                        : typeof(FactoryWithActions<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, ci.DeclaringType, actions);
                }
            }
            else
            {
                if (actions.Length == 0)
                {
                    factoryType = ci.DeclaringType.IsValueType
                        ? typeof(ValueTypeFactoryWithArguments)
                        : typeof(FactoryWithArguments<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, ci, factories);
                }
                else
                {
                    factoryType = ci.DeclaringType.IsValueType
                        ? typeof(ValueTypeFactoryWithArgumentsAndActions)
                        : typeof(FactoryWithArgumentsAndActions<>).MakeGenericType(ci.DeclaringType);
                    instance = Activator.CreateInstance(factoryType, ci, factories, actions);
                }
            }

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), ci.DeclaringType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return (Func<IResolver, object>)Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
        }

        private interface IFactory<out T>
        {
            T Create(IResolver resolver);
        }

        private sealed class Factory<T> : IFactory<T>
        {
            private readonly Type type;

            public Factory(Type type)
            {
                this.type = type;
            }

            public T Create(IResolver resolver)
            {
                return (T)Activator.CreateInstance(type);
            }
        }

        private sealed class ValueTypeFactory : IFactory<object>
        {
            private readonly Type type;

            public ValueTypeFactory(Type type)
            {
                this.type = type;
            }

            public object Create(IResolver resolver)
            {
                return Activator.CreateInstance(type);
            }
        }

        private sealed class FactoryWithActions<T> : IFactory<T>
        {
            private readonly Type type;

            private readonly Action<IResolver, object>[] actions;

            public FactoryWithActions(Type type, Action<IResolver, object>[] actions)
            {
                this.type = type;
                this.actions = actions;
            }

            public T Create(IResolver resolver)
            {
                var obj = (T)Activator.CreateInstance(type);
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](resolver, obj);
                }

                return obj;
            }
        }

        private sealed class ValueTypeFactoryWithActions : IFactory<object>
        {
            private readonly Type type;

            private readonly Action<IResolver, object>[] actions;

            public ValueTypeFactoryWithActions(Type type, Action<IResolver, object>[] actions)
            {
                this.type = type;
                this.actions = actions;
            }

            public object Create(IResolver resolver)
            {
                var obj = Activator.CreateInstance(type);
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

        private sealed class ValueTypeFactoryWithArguments : IFactory<object>
        {
            private readonly ConstructorInfo ci;

            private readonly Func<IResolver, object>[] factories;

            public ValueTypeFactoryWithArguments(ConstructorInfo ci, Func<IResolver, object>[] factories)
            {
                this.ci = ci;
                this.factories = factories;
            }

            public object Create(IResolver resolver)
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](resolver);
                }

                return ci.Invoke(args);
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

        private sealed class ValueTypeFactoryWithArgumentsAndActions : IFactory<object>
        {
            private readonly ConstructorInfo ci;

            private readonly Func<IResolver, object>[] factories;

            private readonly Action<IResolver, object>[] actions;

            public ValueTypeFactoryWithArgumentsAndActions(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
            {
                this.ci = ci;
                this.factories = factories;
                this.actions = actions;
            }

            public object Create(IResolver resolver)
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](resolver);
                }

                var obj = ci.Invoke(args);
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](resolver, obj);
                }

                return obj;
            }
        }

        public Func<IResolver, object> CreateArrayFactory(Type type, Func<IResolver, object>[] factories)
        {
            var arrayType = type.MakeArrayType();

            var factoryType = typeof(ArrayFactory<>).MakeGenericType(type);
            var instance = Activator.CreateInstance(factoryType, new object[] { factories });

            // Make delegate
            var funcType = typeof(Func<,>).MakeGenericType(typeof(IResolver), arrayType);
            // ReSharper disable once AssignNullToNotNullAttribute
            return (Func<IResolver, object>)Delegate.CreateDelegate(funcType, instance, factoryType.GetMethod("Create"));
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
