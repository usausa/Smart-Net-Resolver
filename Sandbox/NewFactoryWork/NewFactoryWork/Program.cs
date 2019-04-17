namespace NewFactoryWork
{
    using System;

    using NewFactoryLib;

    class Program
    {
        static void Main()
        {
            var builder = new Builder();

            var factorySingleton = builder.To(typeof(Singleton).GetConstructors()[0]);
            var singleton = ((Func<IContainer, Singleton>)factorySingleton)(null);
            var singleton2 = ((Func<IContainer, ISingleton>)factorySingleton)(null);

            var factoryTransient = builder.To(typeof(Transient).GetConstructors()[0]);
            var transient = ((Func<IContainer, Transient>)factoryTransient)(null);

            var factoryCombined = builder.To(typeof(Combined).GetConstructors()[0], factorySingleton);
            var combined = ((Func<IContainer, Combined>)factoryCombined)(null);

            var factoryComplex = builder.To(typeof(Complex).GetConstructors()[0], factoryTransient, factoryCombined);
            var complex = ((Func<IContainer, Complex>)factoryComplex)(null);
            var complex2 = ((Func<IContainer, object>)factoryComplex)(null);
        }
    }
}
