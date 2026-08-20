# Smart.Resolver.CompatibilityTest

Verifies the `Smart.Resolver.Extensions.DependencyInjection` adapter against the official
`Microsoft.Extensions.DependencyInjection` specification suite
(`Microsoft.Extensions.DependencyInjection.Specification.Tests`, 10.0.10).

```
dotnet test Smart.Resolver.CompatibilityTest/Smart.Resolver.CompatibilityTest.csproj
```

All 143 specs pass. This document records *how* each part of the M.E.DependencyInjection
contract is satisfied, since most of it is expressed through Smart.Resolver's own
extension points rather than through code that knows about M.E.DependencyInjection.

The guiding rule is that the core resolver never references M.E.DependencyInjection types,
and that every compatibility decision is made once, while a factory is built, so that the
cached delegate a resolution actually runs is unaffected.

## Lifetimes and scopes

`ServiceLifetime` maps onto the existing scopes: `Singleton` to `SingletonScope`, `Scoped`
to `ContainerScope`, `Transient` to no scope at all. `IServiceScope` is a child resolver,
whose `ContainerSlot` already caches and disposes container-scoped instances.

Singletons resolve their dependencies against the root resolver regardless of which scope
first materialised the entry, so a singleton never captures a child resolver that outlives
it (`SmartResolver.CreateFactoryEntry`).

## Enumerable resolution

`IEnumerable<T>` follows M.E.DependencyInjection's slot semantics: one element per
registration, in registration order, closed and open generic registrations merged.
`Binding.Order` records the registration index, and the entry is built by sorting on it.

Elements are not resolved separately: `BindingArrayProvider` asks the kernel for the same
factories a single resolution would use, so an element and an individually resolved service
are the same instance when the lifetime says they should be.

## Open generics

Open generic registrations go through `OpenGenericMissingHandler`. A closed type that
violates the implementation's generic constraints is skipped rather than failing the
resolution, which the handler detects by catching the `ArgumentException` that
`MakeGenericType` raises.

For a single resolution a closed registration wins over one produced from an open generic,
and the last matching registration wins within each group.

## Keyed services

Keys are carried by Smart.Resolver's existing constraint mechanism; the M.E.DependencyInjection
key semantics live entirely in the adapter's `IConstraint` implementations
(`MediKeyConstraint`, `MediAnyKeyConstraint`).

`KeyedService.AnyKey` needs two behaviours the plain key comparison cannot express, both
provided by `IConstraint.IsMultiKey`:

- an `AnyKey` registration is a catch-all that must cache **per key**, so the resolver takes
  a `Scope.Copy` for each keyed entry instead of sharing one scope
- an `AnyKey` registration must be excluded from enumerations, so it is dropped from the
  entry's `Multiple` array while remaining available for single resolution

Querying with `AnyKey` is enumeration-only; a single resolution throws, which the adapter
raises in `GetKeyedService`.

Keyed enumerations work because `ArrayMissingHandler` emits two bindings for an array type:
one with no constraint for the non-keyed case, and one with `MatchAnyConstraint` so that a
keyed request finds the matching elements.

`[ServiceKey]` and parameterless `[FromKeyedServices]` are resolved by two sentinel markers
that `StandardProvider` recognises while building a constructor: `ServiceKeyMarker` bakes the
key in as a constant, and `InheritKeyMarker` resolves the parameter with the key of the
service being constructed. A key that is not assignable to the `[ServiceKey]` parameter makes
that constructor unusable, which surfaces as the `InvalidOperationException` the spec expects.

## Injected IServiceProvider

The provider handed to a constructor must resolve against the requesting scope and must
implement `IKeyedServiceProvider`. Neither requires the core to know the adapter: the
adapter binds `IServiceProvider` to a factory that wraps the resolving context in a keyed
view (`SmartResolverServiceProvider`), whose keyed contract is implemented entirely on the
resolver's own keyed API (`TryGet(type, key)`), including the null-key equivalence and the
`AnyKey` enumeration-only rule.

The view is bound `InContainerScope`, so each scope caches one view in its own slot: every
injection within a scope receives the same instance and allocates nothing after the first.
A singleton's dependencies materialise once while its factory is built, so a singleton
receives one view for its lifetime. No association is stored anywhere - the core resolver
carries no reference to any adapter object. The core's own default binding remains a
constant root resolver, which resolves without a delegate call.

## Service queries

`IServiceProviderIsService` / `IServiceProviderIsKeyedService` are answered by
`SmartServiceProviderIsService` through `IResolver.CanGet`, which reports a materialised
entry's flag when there is one and otherwise probes the binding table structurally. It never
builds a factory, so asking about a registered-but-unconstructable service answers instead of
throwing.

## Disposal

Two behaviours are needed that cost resolution speed, so both are opt-in through
`ResolverOption` and both are enabled by the adapter:

```csharp
public sealed class ResolverOption
{
    public bool DisposalTracking { get; init; }

    public bool RootScope { get; init; }
}
```

`DisposalTracking` makes the owning resolver or scope dispose what it created, in reverse
creation order. Only bindings that can produce an `IDisposable` are wrapped: `IProvider`
declares whether its instances are candidates (`DisposalTracking.ByType` for
`StandardProvider`, whose `TargetType` is the exact runtime type, `Never` for constants and
arrays), so services that cannot be disposable keep the delegates they would have had with
tracking off.

Scopes take part through `IScope.TransferDisposal()`. A scope that returns `true` hands
disposal of its instances to the resolver and stops disposing them itself; the resolver then
captures each instance inside the creation callback, which a scope runs exactly once per
instance. Built-in and custom scopes are treated identically here, and a scope that keeps
ownership is simply never tracked, so nothing is disposed twice.

`RootScope` makes the root provider behave as the root scope, so scoped services resolved
from it are cached once instead of being created per call. It is implemented by resolving
through a dedicated child resolver, and the two shapes are separate types
(`SmartServiceProvider`, `SmartRootScopeServiceProvider`) so that neither carries a branch
for the other.

Instances registered as existing objects are never disposed, matching M.E.DependencyInjection's
ownership rule.

## Behaviour outside the spec

- Within one scope, tracked transients are disposed in reverse creation order first, then
  container-scoped instances; no order is defined between the two groups.
- Types implementing only `IAsyncDisposable` are not tracked.
