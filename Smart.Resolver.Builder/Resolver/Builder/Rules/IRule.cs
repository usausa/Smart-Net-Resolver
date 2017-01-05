namespace Smart.Resolver.Builder.Rules
{
    /// <summary>
    ///
    /// </summary>
    public interface IRule
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool Match(string path);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        void OnBegin(BuilderContext context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        void OnEnd(BuilderContext context);
    }
}
