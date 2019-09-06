namespace Smart.Resolver.Builders
{
    using System;
    using System.Reflection;

    public sealed class ReflectionFactoryBuilder : IFactoryBuilder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
        {
            if (ci.GetParameters().Length == 0)
            {
                return actions.Length == 0
                    ? BuildActivatorFactory(ci.DeclaringType)
                    : BuildActivatorWithActionsFactory(ci.DeclaringType, actions);
            }

            return actions.Length == 0
                ? BuildConstructorFactory(ci, factories)
                : BuildConstructorWithActionsFactory(ci, factories, actions);
        }

        private static Func<IResolver, object> BuildActivatorFactory(Type type)
        {
            return r => Activator.CreateInstance(type);
        }

        private static Func<IResolver, object> BuildActivatorWithActionsFactory(Type type, Action<IResolver, object>[] actions)
        {
            return r =>
            {
                var obj = Activator.CreateInstance(type);

                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, obj);
                }

                return obj;
            };
        }

        private static Func<IResolver, object> BuildConstructorFactory(ConstructorInfo ci, Func<IResolver, object>[] factories)
        {
            return r =>
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](r);
                }

                return ci.Invoke(args);
            };
        }

        private static Func<IResolver, object> BuildConstructorWithActionsFactory(ConstructorInfo ci, Func<IResolver, object>[] factories, Action<IResolver, object>[] actions)
        {
            return r =>
            {
                var args = new object[factories.Length];
                for (var i = 0; i < factories.Length; i++)
                {
                    args[i] = factories[i](r);
                }

                var obj = ci.Invoke(args);

                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](r, obj);
                }

                return obj;
            };
        }

        public Func<IResolver, object> CreateArrayFactory(Type type, Func<IResolver, object>[] factories)
        {
            return r =>
            {
                var array = Array.CreateInstance(type, factories.Length);
                for (var i = 0; i < factories.Length; i++)
                {
                    array.SetValue(factories[i](r), i);
                }

                return array;
            };
        }
    }
}
