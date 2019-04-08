namespace Smart.Resolver
{
    using System;
    using System.Diagnostics;

    using Smart.Resolver.Mocks;

    using Xunit;

    public class BenchmarkTest
    {
        [Fact(Skip = "Benchmark")]
        public void BenchmarkScenarioForWebControllerAndService()
        {
            const int count = 100 * 10000;

            var config = new ResolverConfig();
            config.Bind<IService>().To<Service>().InSingletonScope();
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                resolver.Get<Controller>();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                var gc0 = GC.CollectionCount(0);
                var gc1 = GC.CollectionCount(1);
                var gc2 = GC.CollectionCount(2);

                var start = DateTime.Now;

                for (var i = 0; i < count; i++)
                {
                    resolver.Get<Controller>();
                }

                var time = (DateTime.Now - start).TotalMilliseconds;

                Trace.WriteLine("Count: " + count);
                Trace.WriteLine("Time: " + time);
                Trace.WriteLine("GC0: " + (GC.CollectionCount(0) - gc0));
                Trace.WriteLine("GC1: " + (GC.CollectionCount(1) - gc1));
                Trace.WriteLine("GC2: " + (GC.CollectionCount(2) - gc2));
            }
        }
    }
}
