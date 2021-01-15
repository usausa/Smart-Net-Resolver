namespace RequestKeyBenchmark
{
    using System;
    using System.Collections.Generic;

    using BenchmarkDotNet.Attributes;

    using RequestKeyBenchmark.Constraints;

    using Smart.Collections.Concurrent;

    [Config(typeof(BenchmarkConfig))]
    public class Benchmark
    {
        private readonly ThreadsafeTypeHashArrayMap<object> typeMap = new ThreadsafeTypeHashArrayMap<object>();

        private readonly ThreadsafeHashArrayMap<RequestKey, object> requestKeyMap = new ThreadsafeHashArrayMap<RequestKey, object>();

        private readonly ThreadsafeHashArrayMap<RequestKey2, object> requestKeyMap2 = new ThreadsafeHashArrayMap<RequestKey2, object>(new RequestKey2Comparer());

        private readonly ThreadsafeHashArrayMap<RequestKey3, object> requestKeyMap3 = new ThreadsafeHashArrayMap<RequestKey3, object>();

        private readonly ThreadsafeHashArrayMap<Tuple<Type, IConstraint>, object> tupleMap = new ThreadsafeHashArrayMap<Tuple<Type, IConstraint>, object>();

        private readonly Type key = typeof(Class01);

        [GlobalSetup]
        public void Setup()
        {
            foreach (var type in Classes.Types)
            {
                typeMap.AddIfNotExist(type, new object());
                requestKeyMap.AddIfNotExist(new RequestKey(type, new NameConstraint(string.Empty)), new object());
                requestKeyMap2.AddIfNotExist(new RequestKey2(type, new NameConstraint(string.Empty)), new object());
                requestKeyMap3.AddIfNotExist(new RequestKey3(type, new NameConstraint(string.Empty)), new object());
                tupleMap.AddIfNotExist(new Tuple<Type, IConstraint>(type, new NameConstraint(string.Empty)), new object());
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
            return requestKeyMap.GetValueOrDefault(new RequestKey(key, new NameConstraint(string.Empty)));
        }

        [Benchmark]
        public object RequestKeyMap2()
        {
            return requestKeyMap2.GetValueOrDefault(new RequestKey2(key, new NameConstraint(string.Empty)));
        }


        [Benchmark]
        public object RequestKeyMap3()
        {
            return requestKeyMap3.GetValueOrDefault(new RequestKey3(key, new NameConstraint(string.Empty)));
        }


        [Benchmark]
        public object TupleMap()
        {
            return tupleMap.GetValueOrDefault(new Tuple<Type, IConstraint>(key, new NameConstraint(string.Empty)));
        }
    }

    internal sealed class RequestKey
    {
        public Type Type { get; }

        public IConstraint Constraint { get; }

        public RequestKey(Type type, IConstraint constraint)
        {
            Type = type;
            Constraint = constraint;
        }

        public override bool Equals(object obj)
        {
            return obj is RequestKey other && Type == other.Type && Constraint.Equals(other.Constraint);
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            hash = hash ^ Constraint.GetHashCode();
            return hash;
        }
    }

    public sealed class RequestKey2
    {
        public Type Type { get; }

        public IConstraint Constraint { get; }

        public RequestKey2(Type type, IConstraint constraint)
        {
            Type = type;
            Constraint = constraint;
        }
    }

    public sealed class RequestKey2Comparer : IEqualityComparer<RequestKey2>
    {
        public bool Equals(RequestKey2 x, RequestKey2 y)
        {
            return ((x.Type == y.Type) &&
                    (((x.Constraint is null) && (y.Constraint is null)) ||
                     ((x.Constraint != null) && (y.Constraint != null) && x.Constraint.Equals(y.Constraint))));
        }

        public int GetHashCode(RequestKey2 obj)
        {
            var hash = obj.Type.GetHashCode();
            if (obj.Constraint != null)
            {
                hash = hash ^ obj.Constraint.GetHashCode();
            }
            return hash;
        }
    }

    internal struct RequestKey3 : IEquatable<RequestKey3>
    {
        public Type Type { get; }

        public IConstraint Constraint { get; }

        public RequestKey3(Type type, IConstraint constraint)
        {
            Type = type;
            Constraint = constraint;
        }

        public bool Equals(RequestKey3 other)
        {
            return Type == other.Type && Constraint.Equals(other.Constraint);
        }

        public override bool Equals(object obj)
        {
            return obj is RequestKey3 other && Type == other.Type && Constraint.Equals(other.Constraint);
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            hash = hash ^ Constraint.GetHashCode();
            return hash;
        }
    }
}
