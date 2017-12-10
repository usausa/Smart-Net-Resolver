namespace WorkFactoryResolver
{
    using System;

    public interface IResolver
    {
        void RegisterSingleton(Type type);

        void RegisterTransient(Type type);

        object Get(Type type);
    }

    //--------------------------------------------------------------------------------
    // ObjectResolver
    //--------------------------------------------------------------------------------

    public interface IObjectProvider
    {
        object Create(IResolver resolver);
    }

    public sealed class ObjectResolver : IResolver
    {
        public void RegisterSingleton(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterTransient(Type type)
        {
            throw new NotImplementedException();
        }

        public object Get(Type type)
        {
            throw new NotImplementedException();
        }
    }

    //--------------------------------------------------------------------------------
    // FactoryResolver
    //--------------------------------------------------------------------------------

    public interface IObjectFactory
    {
        object Create();
    }

    public interface IFactoryProvider
    {
        IObjectFactory Resolve(IResolver resolver);
    }

    public sealed class FactoryResolver : IResolver
    {
        public void RegisterSingleton(Type type)
        {
            throw new NotImplementedException();
        }

        public void RegisterTransient(Type type)
        {
            throw new NotImplementedException();
        }

        public object Get(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
