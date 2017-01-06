namespace Smart.Resolver.Builder.Rules
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class DictionaryEntryRule : RuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override bool Match(string path)
        {
            return path.EndsWith("/entry", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            string key;
            if (!context.ElementInfo.Attributes.TryGetValue("key", out key))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Entry element need key attribute. path = [{0}]", context.Path));
            }

            string value;
            if (!context.ElementInfo.Attributes.TryGetValue("value", out value))
            {
                value = context.ElementInfo.Body;
            }

            var dictionary = context.PeekStack<DictionaryStack>();
            if (dictionary == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            dictionary.Add(key, value);
        }
    }
}
