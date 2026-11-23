namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;

internal static class LinkerFlags
{
    public const DynamicallyAccessedMemberTypes Binding = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties;

    public const DynamicallyAccessedMemberTypes Injection = DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties;
}
