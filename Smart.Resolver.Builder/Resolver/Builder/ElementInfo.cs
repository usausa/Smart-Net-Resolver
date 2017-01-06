namespace Smart.Resolver.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Smart.Collections.Generic;
    using Smart.Functional;

    /// <summary>
    ///
    /// </summary>
    public class ElementInfo
    {
        private readonly Dictionary<string, string> attributes;

        private StringBuilder body;

        /// <summary>
        ///
        /// </summary>
        public string Name { get; }

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
            this.attributes = attributes;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAttribute(string key)
        {
            return attributes.GetOrDefault(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Type GetAttributeAsType(string key)
        {
            return attributes.GetOrDefault(key).Or(x => Type.GetType(x, true));
        }
    }
}
