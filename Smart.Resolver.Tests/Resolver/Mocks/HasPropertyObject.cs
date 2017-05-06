namespace Smart.Resolver.Mocks
{
    using Smart.Resolver.Attributes;

    public class HasPropertyObject
    {
        [Inject]
        public SimpleObject Injected { get; set; }
    }
}
