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
        /// <param name="ctx"></param>
        void OnBegin(BuilderContext ctx);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        void OnEnd(BuilderContext ctx);
    }
}
