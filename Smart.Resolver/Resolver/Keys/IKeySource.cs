namespace Smart.Resolver.Keys;

using System.Reflection;

public interface IKeySource
{
    object? GetValue(ICustomAttributeProvider provider);
}
