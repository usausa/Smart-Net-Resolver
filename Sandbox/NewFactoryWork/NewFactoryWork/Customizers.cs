namespace NewFactoryWork
{
    using System;

    using NewFactoryLib;

    public interface IInitialize
    {
        void Initialize();
    }

    public static class ActionBuilder
    {
        public static Action<IContainer, IInitialize> Create1()
        {
            return (c, o) => o.Initialize();
        }
        //public static Action<IContainer, object> Create1()
        //{
        //    return (c, o) => ((IInitialize)o).Initialize();
        //}

        public static Action<IContainer, object> Create2()
        {
            return (c, o) => System.Diagnostics.Debug.WriteLine("Called");
        }
    }
}
