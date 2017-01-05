namespace Smart.Resolver.Builder.Rules
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class ArrayRule : RuleBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/array", StringComparison.OrdinalIgnoreCase);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnBegin(BuilderContext ctx)
        {
            // TODO 属性Type指定 / スタックの値
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext ctx)
        {
            // TODO
        }
    }
}
