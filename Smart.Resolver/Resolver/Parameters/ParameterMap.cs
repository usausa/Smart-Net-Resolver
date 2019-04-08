namespace Smart.Resolver.Parameters
{
    using System.Collections.Generic;

    public sealed class ParameterMap
    {
        private readonly IDictionary<string, IParameter> parameters;

        public ParameterMap(IDictionary<string, IParameter> parameters)
        {
            this.parameters = parameters;
        }

        public IParameter GetParameter(string name)
        {
            return parameters != null && parameters.TryGetValue(name, out var parameter) ? parameter : null;
        }
    }
}
