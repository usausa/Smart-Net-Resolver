namespace Smart.Resolver.Builder
{
    using System;

    using Smart.Resolver.Parameters;

    /// <summary>
    ///
    /// </summary>
    public class NamedParameter : IParameter
    {
        private readonly Type type;

        private readonly string name;

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public NamedParameter(Type type, string name)
        {
            this.type = type;
            this.name = name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public object Resolve(IKernel kernel)
        {
            return kernel.Get(type, name);
        }
    }
}
