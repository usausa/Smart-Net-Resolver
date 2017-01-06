namespace Smart.Resolver.Builder
{
    using System;

    using Smart.Resolver.Builder.Handlers;

    /// <summary>
    ///
    /// </summary>
    public class HandlerEntry
    {
        public Func<string, bool> Matcher { get; }

        public IElementHandler Handler { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="matcher"></param>
        /// <param name="handler"></param>
        public HandlerEntry(Func<string, bool> matcher, IElementHandler handler)
        {
            Matcher = matcher;
            Handler = handler;
        }
    }
}
