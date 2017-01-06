namespace Smart.Resolver.Builder.Handlers.Parameters
{
    using System;
    using System.Globalization;

    using Smart.Resolver.Builder.Stacks;

    /// <summary>
    ///
    /// </summary>
    public class DictionaryEntryHandler : ElementHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public override void OnEnd(BuilderContext context)
        {
            var dictionary = context.PeekStack<DictionaryStack>();
            if (dictionary == null)
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Invalid stack. path = [{0}]", context.Path));
            }

            var key = context.ElementInfo.GetAttribute("key");
            if (String.IsNullOrEmpty(key))
            {
                throw new XmlConfigException(String.Format(CultureInfo.InvariantCulture, "Entry element need key attribute. path = [{0}]", context.Path));
            }

            var value = context.ElementInfo.GetAttribute("value");
            if (String.IsNullOrEmpty(value))
            {
                value = context.ElementInfo.Body;
            }

            dictionary.Add(key, value);
        }
    }
}
