using RequestKeyBenchmark.Constraints;

namespace RequestKeyBenchmark
{
    using System;
    using BenchmarkDotNet.Attributes;
    using Smart.Collections.Concurrent;

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly ThreadsafeTypeHashArrayMap<object> typeMap = new ThreadsafeTypeHashArrayMap<object>();

        private readonly ThreadsafeHashArrayMap<RequestKey, object> requestKeyMap = new ThreadsafeHashArrayMap<RequestKey, object>(new RequestKeyComparer());

        private readonly ThreadsafeHashArrayMap<Tuple<Type, IConstraint>, object> tupleMap = new ThreadsafeHashArrayMap<Tuple<Type, IConstraint>, object>();

        private readonly Type key = typeof(Class01);

        [GlobalSetup]
        public void Setup()
        {
            foreach (var type in Classes.Types)
            {
                typeMap.AddIfNotExist(type, new object());
                requestKeyMap.AddIfNotExist(new RequestKey(type, null), new object());
                tupleMap.AddIfNotExist(new Tuple<Type, IConstraint>(type, null), new object());
            }
        }

        [Benchmark]
        public object TypeMap()
        {
            return typeMap.GetValueOrDefault(key);
        }

        [Benchmark]
        public object RequestKeyMap()
        {
            return requestKeyMap.GetValueOrDefault(new RequestKey(key, null));
        }

        [Benchmark]
        public object TupleMap()
        {
            return tupleMap.GetValueOrDefault(new Tuple<Type, IConstraint>(key, null));
        }
    }
}
