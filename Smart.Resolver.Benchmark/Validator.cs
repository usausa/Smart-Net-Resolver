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
            var singleton11 = (ISingleton1)solver(RequestTypes.Singleton1);
            var singleton12 = (ISingleton1)solver(RequestTypes.Singleton1);

            if (singleton11 != singleton12)
            {
                throw new Exception("Validation error of singleton.");
            }

            var singleton21 = (ISingleton2)solver(RequestTypes.Singleton2);
            var singleton22 = (ISingleton2)solver(RequestTypes.Singleton2);

            if (singleton21 != singleton22)
            {
                throw new Exception("Validation error of singleton.");
            }

            var singleton31 = (ISingleton3)solver(RequestTypes.Singleton3);
            var singleton32 = (ISingleton3)solver(RequestTypes.Singleton3);

            if (singleton31 != singleton32)
            {
                throw new Exception("Validation error of singleton.");
            }

            // Transient
            var transient11 = (ITransient1)solver(RequestTypes.Transient1);
            var transient12 = (ITransient1)solver(RequestTypes.Transient1);

            if (transient11 == transient12)
            {
                throw new Exception("Validation error of transient.");
            }

            var transient21 = (ITransient2)solver(RequestTypes.Transient2);
            var transient22 = (ITransient2)solver(RequestTypes.Transient2);

            if (transient21 == transient22)
            {
                throw new Exception("Validation error of transient.");
            }

            var transient31 = (ITransient3)solver(RequestTypes.Transient3);
            var transient32 = (ITransient3)solver(RequestTypes.Transient3);

            if (transient31 == transient32)
            {
                throw new Exception("Validation error of transient.");
            }

            // Combined
            var combined11 = (Combined1)solver(RequestTypes.Combined1);
            var combined12 = (Combined1)solver(RequestTypes.Combined1);

            if (combined11 == combined12)
            {
                throw new Exception("Validation error of combined.");
            }

            var combined21 = (Combined2)solver(RequestTypes.Combined2);
            var combined22 = (Combined2)solver(RequestTypes.Combined2);

            if (combined21 == combined22)
            {
                throw new Exception("Validation error of combined.");
            }

            var combined31 = (Combined3)solver(RequestTypes.Combined3);
            var combined32 = (Combined3)solver(RequestTypes.Combined3);

            if (combined31 == combined32)
            {
                throw new Exception("Validation error of combined.");
            }

            // Generic
            var generic1 = (IGenericObject<string>)solver(RequestTypes.Generic1);
            var generic2 = (IGenericObject<int>)solver(RequestTypes.Generic2);

            if (generic1 == null)
            {
                throw new Exception("Validation error of combined.");
            }

            if (generic2 == null)
            {
                throw new Exception("Validation error of combined.");
            }

            // MultipleSingleton
            var multipleSingletons = ((IEnumerable<IMultipleSingletonService>)solver(RequestTypes.MultipleSingleton)).ToArray();

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
            var multipleTransients = ((IEnumerable<IMultipleTransientService>)solver(RequestTypes.MultipleTransient)).ToArray();

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
