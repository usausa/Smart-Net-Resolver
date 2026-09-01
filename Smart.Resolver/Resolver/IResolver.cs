namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;

public interface IResolver : IServiceProvider, IDisposable
{
    // CanGet

    bool CanGet<T>();

    bool CanGet<T>(object? key);

    bool CanGet(Type type);

    bool CanGet(Type type, object? key);

    // TryGet

    bool TryGet<T>([MaybeNullWhen(false)] out T obj);

    bool TryGet<T>(object? key, [MaybeNullWhen(false)] out T obj);

    bool TryGet(Type type, [MaybeNullWhen(false)] out object obj);

    bool TryGet(Type type, object? key, [MaybeNullWhen(false)] out object obj);

    // Get

    T Get<T>();

    T Get<T>(object? key);

    object Get(Type type);

    object Get(Type type, object? key);

    // GetAll

    IEnumerable<T> GetAll<T>();

    IEnumerable<T> GetAll<T>(object? key);

    IEnumerable<object> GetAll(Type type);

    IEnumerable<object> GetAll(Type type, object? key);

    // Inject

    void Inject(object instance);
}
