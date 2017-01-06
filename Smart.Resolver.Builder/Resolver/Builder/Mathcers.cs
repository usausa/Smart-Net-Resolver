namespace Smart.Resolver.Builder
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public static class Mathcers
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Func<string, bool> Equals(string path)
        {
            return x => String.Equals(x, path, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Func<string, bool> EndWith(string path)
        {
            return x => x.EndsWith(path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
