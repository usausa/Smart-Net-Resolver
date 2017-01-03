namespace Smart.Resolver.Builder
{
    using System.Collections.Generic;
    using System.Xml;

    using Smart.ComponentModel;
    using Smart.Converter;
    using Smart.Resolver.Builder.Rules;
    using Smart.Resolver.Builder.Scopes;

    /// <summary>
    ///
    /// </summary>
    public class XmlConfigBuilder
    {
        /// <summary>
        ///
        /// </summary>
        public ResolverConfig Config { get; }

        /// <summary>
        ///
        /// </summary>
        public ComponentConfig ComponentConfig { get; } = new ComponentConfig();

        /// <summary>
        ///
        /// </summary>
        public IList<IRule> Rules { get; } = new List<IRule>();

        /// <summary>
        ///
        /// </summary>
        public XmlConfigBuilder()
            : this(new ResolverConfig())
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        public XmlConfigBuilder(ResolverConfig config)
        {
            Config = config;

            ComponentConfig.Add<IObjectConverter>(ObjectConverter.Default);

            ComponentConfig.Add<IScopeHandler, TransientScopeHandler>();
            ComponentConfig.Add<IScopeHandler, SingletonScopeHandler>();

            // TODO Rule!
        }

        //public void Load(string path)
        //{
        //    Load(new XmlTextReader(new StreamReader(path)));
        //}

        //public void Load(TextReader reader)
        //{
        //    Load(new XmlTextReader(reader));
        //}

        //public void Load(XmlNode node)
        //{
        //    Load(new XmlNodeReader(node));
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        public void Load(XmlReader reader)
        {
            var context = new BuilderContext();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    ProcessElement(reader, context);
                }
                else if ((reader.NodeType == XmlNodeType.Text) || (reader.NodeType == XmlNodeType.CDATA))
                {
                    ProcessBody(reader, context);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    ProcessEndElement(reader, context);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        private void ProcessElement(XmlReader reader, BuilderContext context)
        {
            // TODO
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        private void ProcessBody(XmlReader reader, BuilderContext context)
        {
            // TODO
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context"></param>
        private void ProcessEndElement(XmlReader reader, BuilderContext context)
        {
            // TODO
        }
    }
}
