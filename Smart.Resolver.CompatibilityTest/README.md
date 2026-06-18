# Smart.Resolver.CompatibilityTest

English | [日本語](README.ja.md)

Verifies the `Smart.Resolver.Extensions.DependencyInjection` adapter against the official
`Microsoft.Extensions.DependencyInjection` specification suite
(`Microsoft.Extensions.DependencyInjection.Specification.Tests`, 10.0.8).

```
dotnet test Smart.Resolver.CompatibilityTest/Smart.Resolver.CompatibilityTest.csproj
```

Current status: **96 / 143 specs pass** (47 fail).

The `IServiceProviderIsService` / `IServiceProviderIsKeyedService` specs are not counted above:
the adapter does not register those services, so `SupportsIServiceProviderIsService` and
`SupportsIServiceProviderIsKeyedService` return `false` in the test classes.

The adapter types referenced below live in
`Smart.Resolver.Extensions.DependencyInjection/Resolver/`.

## Compatible

Behaviors verified by passing specs:

- [x] Constructor injection, including nested object graphs
- [x] `Transient` / `Singleton` / `Scoped` lifetimes (`ServiceLifetime` → Smart.Resolver scope)
- [x] Scopes via `IServiceScopeFactory` / `IServiceScope` (each scope uses a child resolver)
- [x] `IEnumerable<T>` resolution of multiple **distinct** implementations
- [x] Open generic registrations (`UseOpenGenericBinding`) **without** generic parameter constraints
- [x] Registration by implementation type, factory delegate, and singleton instance
- [x] `[FromKeyedServices]` constructor parameter injection by an explicit key
- [x] Disposal of the provider / scope (the underlying resolver is disposed)

## Not compatible

Each item lists the failing spec(s) and what would be required to support it.

### `IServiceProviderIs(Keyed)Service`

- [ ] `IServiceProviderIsService` / `IServiceProviderIsKeyedService` are not registered
  (the `Supports…` flags are set to `false`, so the specs are skipped).
  **How to address:** bind these services in `SmartServiceProviderFactory.CreateBuilder` to an
  implementation that answers `IsService(Type)` by inspecting the `ResolverConfig` bindings
  (including `IEnumerable<T>` and open generics).

### Keyed services

Keyed services are only partially supported. Resolving by an explicit, matching key works, but
the cases below do not.

- [ ] **Injected `IServiceProvider` cannot resolve keyed services**
  (`SimpleServiceKeyedResolution`, `ResolveKeyed{Transient,Singleton}FromInjectedServiceProvider`).
  An injected `IServiceProvider` resolves to the bare `SmartResolver`, which does not implement
  `IKeyedServiceProvider`, so `GetKeyedService` throws *"This service provider doesn't support
  keyed services."*
  **How to address:** bind `IServiceProvider` / `IKeyedServiceProvider` to the adapter provider
  (`SmartServiceProvider` at the root, `SmartChildServiceProvider` inside a scope) instead of the
  raw resolver, so the injected provider is keyed- and scope-aware.

- [ ] **`KeyedService.AnyKey`** registration and query
  (`ResolveKeyedServicesAnyKey*`, `ResolveKeyedService*WithAnyKey*`, `ResolveWithAnyKeyQuery_*`).
  Keys are matched by equality, so the `AnyKey` sentinel matches nothing.
  **How to address:** special-case `AnyKey` in the keyed binding lookup — treat an `AnyKey`
  registration as matching any requested key, and an `AnyKey` query as matching every keyed
  registration of the type.

- [ ] **Keyed `IEnumerable<T>` resolution**
  (`ResolveKeyedServices`, `ResolveKeyedGenericServices`).
  `UseArrayBinding` only collects non-keyed bindings, so `GetKeyedServices<T>(key)` returns `null`.
  **How to address:** extend array binding to gather all bindings whose key equals the requested key.

- [ ] **`[ServiceKey]` key injection**
  (`ResolveKeyedServiceSingletonInstanceWithKeyInjection`, `...WithKeyedParameter`,
  `ResolveKeyedServiceWithFromServiceKeyAttribute`, `CreateServiceWithKeyedParameter`).
  The key a service was resolved with is not injected into its constructor (a default value is used).
  **How to address:** add an `IKeySource` / parameter source that supplies the current resolution
  key for parameters annotated with `[ServiceKey]` (mirroring `FromKeyedServicesSource`).

- [ ] **`null` key falls back to the non-keyed registration; keyed/non-keyed isolation**
  (`ResolveNullKeyedService`, `ResolveNonKeyedService`, `CombinationalRegistration`).
  A `null` key should resolve the non-keyed service, and keyed-only registrations should not leak
  into non-keyed resolution.
  **How to address:** in `SmartServiceProvider.GetKeyedService` / `GetRequiredKeyedService`, resolve
  without a key when `serviceKey` is `null`, and keep keyed and non-keyed bindings isolated otherwise.

- [ ] **`GetRequiredKeyedService` throws when the service is missing**
  (`ResolveRequiredKeyedServiceThrowsIfNotFound`, `ResolveKeyedServicesAnyKeyConsistency`).
  It currently returns `null` instead of throwing `InvalidOperationException`.
  **How to address:** throw when the resolver yields no instance in the required-resolution paths.

### Generics

- [ ] **Generic parameter constraint filtering for open generics**
  (`ConstrainedOpenGenericServicesCanBeResolved`, `Interface…`, `AbstractClass…`).
  `where T : …` constraints on open generic implementations are not checked, so unsatisfiable
  closings are attempted.
  **How to address:** when closing an open generic, skip bindings whose constraints the requested
  type arguments do not satisfy.

- [ ] **Mixed open + closed generics in one `IEnumerable<T>` (slot order)**
  (`ResolvesMixedOpenClosedGenericsAsEnumerable`, `ResolvingEnumerableContainingOpenGenericServiceUsesCorrectSlot`).
  Closed and open-generic registrations are not merged in registration order.
  **How to address:** build the enumerable from both closed and open-generic bindings while
  preserving registration order.

### Enumerable instancing

- [ ] **Duplicate registrations yield distinct instances**
  (`ResolvesDifferentInstancesForServiceWhenResolvingEnumerable`).
  Registering the same implementation N times should produce N elements, but the array binding
  collapses them to a single cached instance.
  **How to address:** cache singleton / scoped instances per binding rather than per service type,
  so each registration contributes its own element.

### Scopes and disposal

- [ ] **Injected `IServiceProvider` reflects the current scope**
  (`NonSingletonService_WithInjectedProvider_ResolvesScopeProvider`).
  A resolved `IServiceProvider` should be the active scope's provider, not the root.
  **How to address:** register `IServiceProvider` per scope so the child resolver injects its own
  provider (shares the root cause with the keyed injected-provider item above).

- [ ] **Container-tracked disposal of created services**
  (`DisposingScopeDisposesService`, `DisposesInReverseOrderOfCreation`,
  `FactoryServicesAreCreatedAsPartOfCreatingObjectGraph`).
  Transient / factory-created `IDisposable`s are not tracked, so disposing a scope does not dispose
  the services it created, and disposal is not performed in reverse creation order.
  **How to address:** track each created `IDisposable` / `IAsyncDisposable` in its owning scope and
  dispose them in reverse creation order when the scope / provider is disposed.
