namespace Smart.Resolver.Helpers
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public sealed class ArrayFactory
    {
        private readonly Func<int, Array> arrayAllocator;

        private readonly Func<object>[] factories;

        /// <summary>
        ///
        /// </summary>
        /// <param name="arrayAllocator"></param>
        /// <param name="factories"></param>
        public ArrayFactory(Func<int, Array> arrayAllocator, Func<object>[] factories)
        {
            this.arrayAllocator = arrayAllocator;
            this.factories = factories;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public object Create()
        {
            var array = arrayAllocator(factories.Length);
            var objs = (object[])array;
            for (var j = 0; j < factories.Length; j++)
            {
                objs[j] = factories[j]();
            }

            return array;
        }
    }
}
