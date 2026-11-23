namespace Smart.Resolver.Builders;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime;

public sealed class ReflectionFactoryBuilder : IFactoryBuilder
{
    private static readonly bool JitAvailable = JitInfo.GetCompiledMethodCount() > 0;

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "ci is a public parameterless constructor obtained by reflection, so DeclaringType keeps it.")]
    public Func<IResolver, object> CreateFactory(ConstructorInfo ci, Func<IResolver, object?>[] factories, object?[] constants, Action<IResolver, object>[] actions)
    {
        if (ci.GetParameters().Length == 0)
        {
            if (JitAvailable)
            {
                return actions.Length == 0
                    ? BuildActivatorFactory(ci.DeclaringType!)
                    : BuildActivatorWithActionsFactory(ci.DeclaringType!, actions);
            }

            return actions.Length == 0
                ? BuildInvokeFactory(ci)
                : BuildInvokeWithActionsFactory(ci, actions);
        }

        var invoker = ConstructorInvoker.Create(ci);
        return actions.Length == 0
            ? BuildConstructorFactory(invoker, factories, constants)
            : BuildConstructorWithActionsFactory(invoker, factories, constants, actions);
    }

    private static Func<IResolver, object> BuildActivatorFactory(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
    {
        return _ => Activator.CreateInstance(type)!;
    }

    private static Func<IResolver, object> BuildActivatorWithActionsFactory(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type,
        Action<IResolver, object>[] actions)
    {
        return r => Apply(r, Activator.CreateInstance(type)!, actions);
    }

    private static Func<IResolver, object> BuildInvokeFactory(ConstructorInfo ci)
    {
        return _ => ci.Invoke(BindingFlags.DoNotWrapExceptions, null, null, null);
    }

    private static Func<IResolver, object> BuildInvokeWithActionsFactory(ConstructorInfo ci, Action<IResolver, object>[] actions)
    {
        return r => Apply(r, ci.Invoke(BindingFlags.DoNotWrapExceptions, null, null, null), actions);
    }

    private static Func<IResolver, object> BuildConstructorFactory(ConstructorInvoker invoker, Func<IResolver, object?>[] factories, object?[] constants)
    {
        return factories.Length switch
        {
            1 => r => invoker.Invoke(Resolve(r, factories, constants, 0)),
            2 => r => invoker.Invoke(Resolve(r, factories, constants, 0), Resolve(r, factories, constants, 1)),
            3 => r => invoker.Invoke(Resolve(r, factories, constants, 0), Resolve(r, factories, constants, 1), Resolve(r, factories, constants, 2)),
            4 => r => invoker.Invoke(Resolve(r, factories, constants, 0), Resolve(r, factories, constants, 1), Resolve(r, factories, constants, 2), Resolve(r, factories, constants, 3)),
            _ => r => invoker.Invoke(ResolveAll(r, factories, constants))
        };
    }

    private static Func<IResolver, object> BuildConstructorWithActionsFactory(ConstructorInvoker invoker, Func<IResolver, object?>[] factories, object?[] constants, Action<IResolver, object>[] actions)
    {
        return factories.Length switch
        {
            1 => r => Apply(r, invoker.Invoke(Resolve(r, factories, constants, 0)), actions),
            2 => r => Apply(r, invoker.Invoke(Resolve(r, factories, constants, 0), Resolve(r, factories, constants, 1)), actions),
            3 => r => Apply(r, invoker.Invoke(Resolve(r, factories, constants, 0), Resolve(r, factories, constants, 1), Resolve(r, factories, constants, 2)), actions),
            4 => r => Apply(r, invoker.Invoke(Resolve(r, factories, constants, 0), Resolve(r, factories, constants, 1), Resolve(r, factories, constants, 2), Resolve(r, factories, constants, 3)), actions),
            _ => r => Apply(r, invoker.Invoke(ResolveAll(r, factories, constants)), actions)
        };
    }

    private static object? Resolve(IResolver resolver, Func<IResolver, object?>[] factories, object?[] constants, int index) =>
        constants[index] ?? factories[index](resolver);

    private static Span<object?> ResolveAll(IResolver resolver, Func<IResolver, object?>[] factories, object?[] constants)
    {
        var args = new object?[factories.Length];
        for (var i = 0; i < factories.Length; i++)
        {
            args[i] = constants[i] ?? factories[i](resolver);
        }

        return args;
    }

    private static object Apply(IResolver resolver, object obj, Action<IResolver, object>[] actions)
    {
        foreach (var action in actions.AsSpan())
        {
            action(resolver, obj);
        }

        return obj;
    }

    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Arrays of reference types need no dynamic code. For value types, the caller makes the array type available, for example by referencing typeof(T[]).")]
    public Func<IResolver, object> CreateArrayFactory(Type type, Func<IResolver, object?>[] factories)
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
