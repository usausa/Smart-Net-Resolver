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

public sealed class MetadataEntry
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? ValueType { get; set; }
}

public sealed class ParameterEntry
{
    public string Name { get; set; } = string.Empty;
    public ParameterKind Kind { get; set; } = ParameterKind.Constant;
    public string? Value { get; set; }
    public string? ValueType { get; set; }
}

//--------------------------------------------------------------------------------
// Definition
//--------------------------------------------------------------------------------

public sealed class BindingDefinition
{
    public string ServiceType { get; set; } = string.Empty;

    public BindingTargetKind TargetKind { get; set; } = BindingTargetKind.Type;

    public string? ImplementationType { get; set; }

    public string? ConstantValue { get; set; }

    public string? ConstantValueType { get; set; }

    public ScopeKind Scope { get; set; } = ScopeKind.Transient;

    public string? ConstraintKey { get; set; }

    public List<MetadataEntry> Metadata { get; set; } = [];

    public List<ParameterEntry> ConstructorArguments { get; set; } = [];

    public List<ParameterEntry> PropertyValues { get; set; } = [];
}

public sealed class ResolverDefinition
{
    public List<BindingDefinition> Bindings { get; set; } = [];
}
