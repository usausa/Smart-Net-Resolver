namespace NewFactoryWork
{
    using NewFactoryLib;

    class Program
    {
        static void Main()
        {
            var builder = new Builder();

            var factorySingleton = builder.To<Singleton>(typeof(Singleton).GetConstructors()[0]);
            //var singleton = factorySingleton(null);

            var factoryTransient = builder.To<Transient>(typeof(Transient).GetConstructors()[0]);
            //var transient = factoryTransient(null);

            var factoryCombined = builder.To<Combined>(typeof(Combined).GetConstructors()[0], factorySingleton);
            //var combined = factoryCombined(null);

            var factoryComplex = builder.To<Complex>(typeof(Complex).GetConstructors()[0], factoryTransient, factoryCombined);
            var complex = factoryComplex(null);
        }
    }
}
