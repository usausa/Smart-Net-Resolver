namespace Smart.Resolver.Keys;

using System.Reflection;

public static class KeySourceHelper
{
    public static object? GetValue(ICustomAttributeProvider provider, IKeySource[] sources)
    {
        foreach (var source in sources)
        {
            var value = source.GetValue(provider);
            if (value is not null)
            {
                return value;
            }
        }

        return null;
    }
}
