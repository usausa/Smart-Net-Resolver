namespace Smart.Resolver.Injectors;

using System.Diagnostics.CodeAnalysis;

using Smart.Resolver.Bindings;

public interface IInjector
{
    Action<IResolver, object>? CreateInjector([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type type, Binding binding);
}
