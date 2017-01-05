namespace Smart.Resolver.Builder.Stacks
{
    using System;

    public class ActivatorStack
    {
        public Type TargetType { get; }

        public ActivatorStack(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
