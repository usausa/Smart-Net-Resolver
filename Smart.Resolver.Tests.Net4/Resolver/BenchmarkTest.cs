namespace Smart.Resolver
{
    using System;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class BenchmarkTest
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", Justification = "Ignore")]
        [Ignore]
        [TestMethod]
        public void BenchmarkScinarioForWebControllerAndService()
        {
            const int count = 100 * 10000;

            resolver.Bind<IService>().To<Service>().InSingletonScope();
            resolver.Bind<Controller>().ToSelf();

            var start = DateTime.Now;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            for (var i = 0; i < count; i++)
            {
                resolver.Get<Controller>();
            }

            var time = (DateTime.Now - start).TotalMilliseconds;

            Trace.WriteLine("Count: " + count);
            Trace.WriteLine("Time: " + time);
            Trace.WriteLine("GC0: " + GC.CollectionCount(0));
            Trace.WriteLine("GC1: " + GC.CollectionCount(1));
            Trace.WriteLine("GC2: " + GC.CollectionCount(2));
        }
    }
}
