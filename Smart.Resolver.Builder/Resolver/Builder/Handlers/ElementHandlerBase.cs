namespace Smart.Resolver.Builder.Handlers
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ElementHandlerBase : IElementHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnBegin(BuilderContext context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnEnd(BuilderContext context)
        {
        }
    }
}
