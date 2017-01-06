namespace Smart.Resolver.Builder.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public interface IElementHandler
    {
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
