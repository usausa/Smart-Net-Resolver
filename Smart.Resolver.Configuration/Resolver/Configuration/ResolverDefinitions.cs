namespace Smart.Resolver.Configuration;

//--------------------------------------------------------------------------------
// Enums
//--------------------------------------------------------------------------------

public enum BindingTargetKind
{
    Self,
    Type,
    Constant
}

public enum ScopeKind
{
    Transient,
    Singleton,
    Container
}

public enum ParameterKind
{
    Constant,
    Reference
}

//--------------------------------------------------------------------------------
// Entry
//--------------------------------------------------------------------------------

public sealed class ParameterEntry
{
    public string Name { get; set; } = string.Empty;

    public ParameterKind Kind { get; set; } = ParameterKind.Constant;

    public string? ValueType { get; set; }

    public string? Value { get; set; }
}

public sealed class MetadataEntry
{
    public string Key { get; set; } = string.Empty;

    public string? ValueType { get; set; }

    public string? Value { get; set; }
}

//--------------------------------------------------------------------------------
// Definition
//--------------------------------------------------------------------------------

#pragma warning disable CA1002
#pragma warning disable CA2227
public sealed class BindingDefinition
{
    public string? Key { get; set; }

    public ScopeKind Scope { get; set; } = ScopeKind.Transient;

    public BindingTargetKind BindingTarget { get; set; } = BindingTargetKind.Type;

    public string Service { get; set; } = string.Empty;

    public string? Implementation { get; set; }

    public string? ConstantType { get; set; }

    public string? Constant { get; set; }

    public List<ParameterEntry> ConstructorArguments { get; set; } = [];

    public List<ParameterEntry> PropertyValues { get; set; } = [];

    public List<MetadataEntry> Metadata { get; set; } = [];
}

public sealed class ResolverDefinition
{
    public List<BindingDefinition> Bindings { get; set; } = [];
}
#pragma warning restore CA2227
#pragma warning restore CA1002
