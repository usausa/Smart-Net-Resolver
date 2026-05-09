namespace Smart.Resolver.Configuration;

public sealed class ResolverDefinitionException : Exception
{
    public int BindingIndex { get; }

    public string FieldName { get; }

    public ResolverDefinitionException()
        : this(0, string.Empty, string.Empty)
    {
    }

    public ResolverDefinitionException(string message)
        : base(message)
    {
        FieldName = string.Empty;
    }

    public ResolverDefinitionException(string message, Exception innerException)
        : base(message, innerException)
    {
        FieldName = string.Empty;
    }

    public ResolverDefinitionException(int bindingIndex, string fieldName, string message)
        : base($"[Binding #{bindingIndex}] {fieldName}: {message}")
    {
        BindingIndex = bindingIndex;
        FieldName = fieldName;
    }
}
