namespace Smart.Resolver
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Attributes;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class ConstraintTest
    {
        private StandardResolver resolver;

        [TestInitialize]
        public void TestInitialize()
        {
            resolver = new StandardResolver();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            resolver.Dispose();
        }

        [TestMethod]
        public void ObjectIsSelectedByNameConstraint()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            resolver.Bind<NameConstraintInjectedObject>().ToSelf();

            var obj = resolver.Get<NameConstraintInjectedObject>();
            var foo = resolver.Get<SimpleObject>("foo");
            var bar = resolver.Get<SimpleObject>("bar");

            Assert.AreSame(obj.SimpleObject, foo);
            Assert.AreNotSame(obj.SimpleObject, bar);
        }

        [TestMethod]
        public void ObjectIsSelectedByHasMetadataConstraint()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope();
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
            resolver.Bind<HasMetadataConstraintInjectedObject>().ToSelf();

            var obj = resolver.Get<HasMetadataConstraintInjectedObject>();
            var hoge = resolver.Resolve(typeof(SimpleObject), new HasMetadataConstraint("hoge"));

            Assert.AreSame(obj.SimpleObject, hoge);
        }

        [TestMethod]
        public void ObjectIsSelectedByChainConstraint()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope();
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar").WithMetadata("hoge", null);
            resolver.Bind<ChainConstraintInjectedObject>().ToSelf();

            var obj = resolver.Get<ChainConstraintInjectedObject>();
            var barHoge = resolver.Resolve(typeof(SimpleObject), new ChainConstraint(new NameConstraint("bar"), new HasMetadataConstraint("hoge")));

            Assert.AreSame(obj.SimpleObject, barHoge);
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
