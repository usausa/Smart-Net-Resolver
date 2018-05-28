namespace Smart.Resolver.Parameters
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public sealed class ParameterMap
    {
        private readonly IDictionary<string, IParameter> parameters;

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameters"></param>
        public ParameterMap(IDictionary<string, IParameter> parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Extensions")]
        public IParameter GetParameter(string name)
        {
            return parameters != null && parameters.TryGetValue(name, out var parameter) ? parameter : null;
        }
    }
}
