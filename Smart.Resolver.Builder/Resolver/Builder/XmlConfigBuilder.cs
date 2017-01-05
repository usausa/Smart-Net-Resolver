namespace Smart.Resolver.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        public ResolverConfig ResolverConfig { get; }

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
        /// <param name="resolverConfig"></param>
        public XmlConfigBuilder(ResolverConfig resolverConfig)
        {
            ResolverConfig = resolverConfig;

            // Components
            ComponentConfig.Add<IObjectConverter>(ObjectConverter.Default);

            ComponentConfig.Add<IScopeHandler, TransientScopeHandler>();
            ComponentConfig.Add<IScopeHandler, SingletonScopeHandler>();

            // Rules
            Rules.Add(new ComponentRule());

            Rules.Add(new BindingRule());
            Rules.Add(new MetadataRule());
            Rules.Add(new ConstantRule());
            Rules.Add(new FactoryRule());

            Rules.Add(new ConstructorArgRule());
            Rules.Add(new PropertyRule());

            Rules.Add(new ArrayRule());
            Rules.Add(new ListRule());
            Rules.Add(new ValueRule());

            Rules.Add(new DictionaryRule());
            Rules.Add(new EntryRule());

            Rules.Add(new ObjectRule());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reader"></param>
        public void Load(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var context = new BuilderContext(ResolverConfig, ComponentConfig.ToContainer());

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var isEmpty = reader.IsEmptyElement;

                    var name = reader.LocalName;
                    var attributes = new Dictionary<string, string>();
                    while (reader.MoveToNextAttribute())
                    {
                        attributes[reader.Name] = reader.Value;
                    }

                    context.AddElement(new ElementInfo(name, attributes));

                    var rule = FindRule(context.Path);
                    rule?.OnBegin(context);

                    if (isEmpty)
                    {
                        rule?.OnEnd(context);

                        context.RemoveElement();
                    }
                }
                else if ((reader.NodeType == XmlNodeType.Text) || (reader.NodeType == XmlNodeType.CDATA))
                {
                    context.ElementInfo.AddBody(reader.Value);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    var rule = FindRule(context.Path);
                    rule?.OnEnd(context);

                    context.RemoveElement();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IRule FindRule(string path)
        {
            return Rules.FirstOrDefault(rule => rule.Match(path));
        }
    }
}
