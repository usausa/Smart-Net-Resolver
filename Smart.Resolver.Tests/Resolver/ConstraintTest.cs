namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Attributes;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Mocks;

    using Xunit;

    /// <summary>
    ///
    /// </summary>
    public class ConstraintTest
    {
        [Fact]
        public void ObjectIsSelectedByNameConstraint()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            config.Bind<NameConstraintInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<NameConstraintInjectedObject>();
                var foo = resolver.Get<SimpleObject>("foo");
                var bar = resolver.Get<SimpleObject>("bar");

                Assert.Same(obj.SimpleObject, foo);
                Assert.NotSame(obj.SimpleObject, bar);
            }
        }

        [Fact]
        public void ObjectIsSelectedByHasMetadataConstraint()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
            config.Bind<HasMetadataConstraintInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasMetadataConstraintInjectedObject>();
                var hoge = resolver.Resolve(typeof(SimpleObject), new HasMetadataConstraint("hoge"));

                Assert.Same(obj.SimpleObject, hoge);
            }
        }

        [Fact]
        public void ObjectIsSelectedByChainConstraint()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar").WithMetadata("hoge", null);
            config.Bind<ChainConstraintInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<ChainConstraintInjectedObject>();
                var barHoge = resolver.Resolve(typeof(SimpleObject), new ChainConstraint(new NameConstraint("bar"), new HasMetadataConstraint("hoge")));

                Assert.Same(obj.SimpleObject, barHoge);
            }
        }

        protected class HasMetadataConstraint : IConstraint
        {
            public string Key { get; }

            public HasMetadataConstraint(string key)
            {
                Key = key;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
            public bool Match(IBindingMetadata metadata)
            {
                return metadata.Has(Key);
            }
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
        protected sealed class HasMetadataAttribute : ConstraintAttribute
        {
            public string Key { get; }

            public HasMetadataAttribute(string key)
            {
                Key = key;
            }

            public override IConstraint CreateConstraint()
            {
                return new HasMetadataConstraint(Key);
            }
        }

        protected class NameConstraintInjectedObject
        {
            public SimpleObject SimpleObject { get; }

            public NameConstraintInjectedObject([Named("foo")] SimpleObject simpleObject)
            {
                SimpleObject = simpleObject;
            }
        }

        protected class HasMetadataConstraintInjectedObject
        {
            public SimpleObject SimpleObject { get; }

            public HasMetadataConstraintInjectedObject([HasMetadata("hoge")] SimpleObject simpleObject)
            {
                SimpleObject = simpleObject;
            }
        }

        protected class ChainConstraintInjectedObject
        {
            public SimpleObject SimpleObject { get; }

            public ChainConstraintInjectedObject([Named("bar")] [HasMetadata("hoge")] SimpleObject simpleObject)
            {
                SimpleObject = simpleObject;
            }
        }
    }
}
