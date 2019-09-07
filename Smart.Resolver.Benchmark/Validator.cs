namespace Smart.Resolver.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Resolver.Benchmark.Classes;

    public static class Validator
    {
        public static void Validate(Func<Type, object> solver)
        {
            // Singleton
            var singleton11 = (ISingleton1)solver(typeof(ISingleton1));
            var singleton12 = (ISingleton1)solver(typeof(ISingleton1));

            if (singleton11 != singleton12)
            {
                throw new Exception("Validation error of singleton.");
            }

            var singleton21 = (ISingleton2)solver(typeof(ISingleton2));
            var singleton22 = (ISingleton2)solver(typeof(ISingleton2));

            if (singleton21 != singleton22)
            {
                throw new Exception("Validation error of singleton.");
            }

            var singleton31 = (ISingleton3)solver(typeof(ISingleton3));
            var singleton32 = (ISingleton3)solver(typeof(ISingleton3));

            if (singleton31 != singleton32)
            {
                throw new Exception("Validation error of singleton.");
            }

            var singleton41 = (ISingleton4)solver(typeof(ISingleton4));
            var singleton42 = (ISingleton4)solver(typeof(ISingleton4));

            if (singleton41 != singleton42)
            {
                throw new Exception("Validation error of singleton.");
            }

            var singleton51 = (ISingleton5)solver(typeof(ISingleton5));
            var singleton52 = (ISingleton5)solver(typeof(ISingleton5));

            if (singleton51 != singleton52)
            {
                throw new Exception("Validation error of singleton.");
            }

            // Transient
            var transient11 = (ITransient1)solver(typeof(ITransient1));
            var transient12 = (ITransient1)solver(typeof(ITransient1));

            if (transient11 == transient12)
            {
                throw new Exception("Validation error of transient.");
            }

            var transient21 = (ITransient2)solver(typeof(ITransient2));
            var transient22 = (ITransient2)solver(typeof(ITransient2));

            if (transient21 == transient22)
            {
                throw new Exception("Validation error of transient.");
            }

            var transient31 = (ITransient3)solver(typeof(ITransient3));
            var transient32 = (ITransient3)solver(typeof(ITransient3));

            if (transient31 == transient32)
            {
                throw new Exception("Validation error of transient.");
            }

            var transient41 = (ITransient4)solver(typeof(ITransient4));
            var transient42 = (ITransient4)solver(typeof(ITransient4));

            if (transient41 == transient42)
            {
                throw new Exception("Validation error of transient.");
            }

            var transient51 = (ITransient4)solver(typeof(ITransient4));
            var transient52 = (ITransient4)solver(typeof(ITransient4));

            if (transient51 == transient52)
            {
                throw new Exception("Validation error of transient.");
            }

            // Combined
            var combined11 = (Combined1)solver(typeof(Combined1));
            var combined12 = (Combined1)solver(typeof(Combined1));

            if (combined11 == combined12)
            {
                throw new Exception("Validation error of combined.");
            }

            var combined21 = (Combined2)solver(typeof(Combined2));
            var combined22 = (Combined2)solver(typeof(Combined2));

            if (combined21 == combined22)
            {
                throw new Exception("Validation error of combined.");
            }

            var combined31 = (Combined3)solver(typeof(Combined3));
            var combined32 = (Combined3)solver(typeof(Combined3));

            if (combined31 == combined32)
            {
                throw new Exception("Validation error of combined.");
            }

            var combined41 = (Combined4)solver(typeof(Combined4));
            var combined42 = (Combined4)solver(typeof(Combined4));

            if (combined41 == combined42)
            {
                throw new Exception("Validation error of combined.");
            }

            var combined51 = (Combined5)solver(typeof(Combined5));
            var combined52 = (Combined5)solver(typeof(Combined5));

            if (combined51 == combined52)
            {
                throw new Exception("Validation error of combined.");
            }

            // Generic
            var generic1 = (IGenericObject<string>)solver(typeof(IGenericObject<string>));
            var generic2 = (IGenericObject<int>)solver(typeof(IGenericObject<int>));

            if (generic1 is null)
            {
                throw new Exception("Validation error of combined.");
            }

            if (generic2 is null)
            {
                throw new Exception("Validation error of combined.");
            }

            // MultipleSingleton
            var multipleSingletons = ((IEnumerable<IMultipleSingletonService>)solver(typeof(IEnumerable<IMultipleSingletonService>))).ToArray();

            if (multipleSingletons.Length != 5)
            {
                throw new Exception("Validation error of singleton transient.");
            }

            for (var i = 1; i < multipleSingletons.Length; i++)
            {
                if (multipleSingletons[0] == multipleSingletons[i])
                {
                    throw new Exception("Validation error of multiple singleton.");
                }
            }

            // MultipleTransient
            var multipleTransients = ((IEnumerable<IMultipleTransientService>)solver(typeof(IEnumerable<IMultipleTransientService>))).ToArray();

            if (multipleTransients.Length != 5)
            {
                throw new Exception("Validation error of multiple transient.");
            }

            for (var i = 0; i < multipleTransients.Length - 1; i++)
            {
                for (var j = i + 1; j < multipleTransients.Length; j++)
                {
                    if (multipleSingletons[i] == multipleSingletons[j])
                    {
                        throw new Exception("Validation error of multiple transient.");
                    }
                }
            }
        }
    }
}
