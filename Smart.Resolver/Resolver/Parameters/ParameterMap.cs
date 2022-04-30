namespace Smart.Resolver.Parameters;

public sealed class ParameterMap
{
    private readonly IDictionary<string, IParameter>? parameters;

    public ParameterMap(IDictionary<string, IParameter>? parameters)
    {
        this.parameters = parameters;
    }

    public IParameter? GetParameter(string name)
    {
        return parameters is not null && parameters.TryGetValue(name, out var parameter) ? parameter : null;
    }
}
