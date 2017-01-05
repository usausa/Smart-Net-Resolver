namespace Smart.Resolver.Builder
{
    using System;
#if !PCL
    using System.Runtime.Serialization;
#endif

    /// <summary>
    ///
    /// </summary>
#if !PCL
    [Serializable]
#endif
    public class XmlConfigException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public XmlConfigException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public XmlConfigException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public XmlConfigException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if !PCL
        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected XmlConfigException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
