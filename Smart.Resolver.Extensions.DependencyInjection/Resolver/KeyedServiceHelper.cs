namespace Smart.Resolver;

internal static class KeyedServiceHelper
{
    public static bool IsEnumerableService(Type serviceType) =>
        serviceType.IsGenericType && (serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));
}
