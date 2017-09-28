﻿namespace Smart.Resolver.Providers
{
    using System;
    using Smart.ComponentModel;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        ///
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        object Create(IKernel kernel, IBinding binding);

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IProvider Copy(IComponentContainer components);
    }
}
