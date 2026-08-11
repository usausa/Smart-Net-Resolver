namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

internal static class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowServiceNotRegistered(Type serviceType) =>
        throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "No service for type '{0}' has been registered.", serviceType));

    [DoesNotReturn]
    public static void ThrowKeyedServiceNotRegistered(Type serviceType, object? serviceKey) =>
        throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "No service for type '{0}' and key '{1}' has been registered.", serviceType, serviceKey));

    [DoesNotReturn]
    public static void ThrowAnyKeyNotSupported() =>
        throw new InvalidOperationException("KeyedService.AnyKey cannot be used to resolve a single service.");
}
