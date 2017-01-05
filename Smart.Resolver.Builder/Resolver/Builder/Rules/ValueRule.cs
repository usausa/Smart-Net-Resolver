namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class ValueRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/value", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext ctx)
        {
            var stack = ctx.PeekObject() as ListStack;
            if (stack == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid path. [{0}]", ctx.Path));
            }

            stack.Add(ctx.ElementInfo.Body);
        }
    }
}
