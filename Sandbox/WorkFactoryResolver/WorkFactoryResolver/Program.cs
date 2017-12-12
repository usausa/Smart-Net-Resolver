namespace WorkFactoryResolver
{
    using System.Reflection;

    using BenchmarkDotNet.Running;

    public static class Program
    {
        public static void Main(string[] args)
        {
            //var resolver = new ObjectResolver();
            //resolver.RegisterSingleton(typeof(Service1));
            //resolver.RegisterSingleton(typeof(Service2));
            //resolver.RegisterSingleton(typeof(Service3));
            //resolver.RegisterTransient(typeof(SubObject1));
            //resolver.RegisterTransient(typeof(SubObject2));
            //resolver.RegisterTransient(typeof(SubObject3));
            //resolver.RegisterTransient(typeof(Complex));

            //var c1 = (Complex)resolver.Get(typeof(Complex));
            //var c2 = (Complex)resolver.Get(typeof(Complex));
            //System.Diagnostics.Debug.Assert(c1 != c2);
            //System.Diagnostics.Debug.Assert(c1.Service1 == c2.Service1);
            //System.Diagnostics.Debug.Assert(c1.Service2 == c2.Service2);
            //System.Diagnostics.Debug.Assert(c1.Service3 == c2.Service3);
            //System.Diagnostics.Debug.Assert(c1.SubObject1 != c2.SubObject1);
            //System.Diagnostics.Debug.Assert(c1.SubObject2 != c2.SubObject2);
            //System.Diagnostics.Debug.Assert(c1.SubObject3 != c2.SubObject3);


            BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);
        }
    }
}
