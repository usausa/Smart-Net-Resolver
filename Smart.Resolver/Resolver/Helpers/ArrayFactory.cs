namespace Smart.Resolver.Helpers
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public static class ArrayFactory
    {
        public static Func<object> Create(Func<int, Array> arrayAllocator, Func<object>[] factories)
        {
            return () =>
            {
                var array = arrayAllocator(factories.Length);
                var objs = (object[])array;
                for (var i = 0; i < factories.Length; i++)
                {
                    objs[i] = factories[i]();
                }

                return array;
            };
        }
    }
}
