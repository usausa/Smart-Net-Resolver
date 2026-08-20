namespace Smart.Resolver;

// Inspects the shape of a requested service type
internal static class ServiceTypeHelper
{
    public static bool IsEnumerableService(Type serviceType) =>
        serviceType.IsGenericType && (serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>));
}
