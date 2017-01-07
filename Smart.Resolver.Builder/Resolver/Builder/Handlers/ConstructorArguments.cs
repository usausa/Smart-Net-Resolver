namespace Smart.Resolver.Builder.Handlers
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public class ConstructorArguments
    {
        private readonly Dictionary<string, object> namedParameters = new Dictionary<string, object>();

        private readonly Dictionary<int, object> indexedParameters = new Dictionary<int, object>();

        /// <summary>
        ///
        /// </summary>
        public int Count => namedParameters.Count + indexedParameters.Count;

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddNamedParameter(string name, object value)
        {
            namedParameters[name] = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void AddIndexedParameter(int index, object value)
        {
            indexedParameters[index] = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Ignore")]
        public bool TryGetNamadParameter(string name, out object value)
        {
            return namedParameters.TryGetValue(name, out value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate", Justification = "Ignore")]
        public bool TryGetIndexedParameter(int index, out object value)
        {
            return indexedParameters.TryGetValue(index, out value);
        }
    }
}
