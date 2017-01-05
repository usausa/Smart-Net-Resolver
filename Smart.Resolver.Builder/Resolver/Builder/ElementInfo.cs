namespace Smart.Resolver.Builder
{
    using System.Collections.Generic;
    using System.Text;

    public class ElementInfo
    {
        private StringBuilder body;

        /// <summary>
        ///
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> Attributes { get; }

        /// <summary>
        ///
        /// </summary>
        public string Body => body?.ToString().Trim();

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        public ElementInfo(string name, Dictionary<string, string> attributes)
        {
            Name = name;
            Attributes = attributes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        public void AddBody(string text)
        {
            if (body == null)
            {
                body = new StringBuilder();
            }

            body.Append(text);
        }
    }
}
