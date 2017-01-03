namespace Smart.Resolver.Builder
{
    using System.Collections.Generic;
    using System.Text;

    public class ElementInfo
    {
        private Dictionary<string, string> attributes;

        private StringBuilder body;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> Attributes => attributes ?? (attributes = new Dictionary<string, string>());

        /// <summary>
        ///
        /// </summary>
        public StringBuilder Body => body ?? (body = new StringBuilder());
    }
}
