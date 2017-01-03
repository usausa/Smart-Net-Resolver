namespace Smart.Resolver.Builder.Rules
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RuleBase : IRule
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract bool Match(string path);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void OnBegin(BuilderContext ctx)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void OnEnd(BuilderContext ctx)
        {
        }
    }
}
