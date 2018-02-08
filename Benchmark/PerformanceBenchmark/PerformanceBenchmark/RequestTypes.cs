namespace PerformanceBenchmark
{
    using System;
    using System.Collections.Generic;

    using PerformanceBenchmark.Classes;

    public static class RequestTypes
    {
        public static readonly Type Singleton1 = typeof(ISingleton1);
        public static readonly Type Singleton2 = typeof(ISingleton2);
        public static readonly Type Singleton3 = typeof(ISingleton3);

        public static readonly Type Transient1 = typeof(ITransient1);
        public static readonly Type Transient2 = typeof(ITransient2);
        public static readonly Type Transient3 = typeof(ITransient3);

        public static readonly Type Combined1 = typeof(Combined1);
        public static readonly Type Combined2 = typeof(Combined2);
        public static readonly Type Combined3 = typeof(Combined3);

        public static readonly Type Generic1 = typeof(IGenericObject<string>);

        public static readonly Type Generic2 = typeof(IGenericObject<int>);

        public static readonly Type MultipleSinglton = typeof(IEnumerable<IMultpleSingletonService>);

        public static readonly Type MultipleTransient = typeof(IEnumerable<IMultpleTransientService>);
    }
}
