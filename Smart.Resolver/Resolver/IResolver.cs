namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;

#pragma warning disable CA1716
public interface IResolver : IServiceProvider, IDisposable
{
    // CanGet

    bool CanGet<T>();

    bool CanGet<T>(object? parameter);

    bool CanGet(Type type);

    bool CanGet(Type type, object? parameter);

    // TryGet

    bool TryGet<T>([MaybeNullWhen(false)] out T obj);

    bool TryGet<T>(object? parameter, [MaybeNullWhen(false)] out T obj);

    bool TryGet(Type type, [MaybeNullWhen(false)] out object obj);

    bool TryGet(Type type, object? parameter, [MaybeNullWhen(false)] out object obj);

    // Get

    T Get<T>();

    T Get<T>(object? parameter);

    object Get(Type type);

    object Get(Type type, object? parameter);

    // GetAll

    IEnumerable<T> GetAll<T>();

    IEnumerable<T> GetAll<T>(object? parameter);

    IEnumerable<object> GetAll(Type type);

    IEnumerable<object> GetAll(Type type, object? parameter);

    // Inject

    void Inject(object instance);
}
#pragma warning restore CA1716
