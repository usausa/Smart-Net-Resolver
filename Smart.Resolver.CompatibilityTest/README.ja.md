# Smart.Resolver.CompatibilityTest

[English](README.md) | 日本語

`Smart.Resolver.Extensions.DependencyInjection` アダプターを、公式の
`Microsoft.Extensions.DependencyInjection` 仕様テストスイート
（`Microsoft.Extensions.DependencyInjection.Specification.Tests`、10.0.8）に対して検証します。

```
dotnet test Smart.Resolver.CompatibilityTest/Smart.Resolver.CompatibilityTest.csproj
```

現在の状況: **143 件中 96 件が合格**（47 件が失敗）。

`IServiceProviderIsService` / `IServiceProviderIsKeyedService` の仕様は上記の件数に含めていません。
アダプターがこれらのサービスを登録しないため、テストクラスでは
`SupportsIServiceProviderIsService` および `SupportsIServiceProviderIsKeyedService` が `false` を返します。

以下で参照するアダプターの型は
`Smart.Resolver.Extensions.DependencyInjection/Resolver/` にあります。

## 互換性あり

合格している仕様で検証済みの動作:

- [x] コンストラクター注入（ネストしたオブジェクトグラフを含む）
- [x] `Transient` / `Singleton` / `Scoped` のライフタイム（`ServiceLifetime` → Smart.Resolver のスコープ）
- [x] `IServiceScopeFactory` / `IServiceScope` によるスコープ（各スコープは子リゾルバーを使用）
- [x] 複数の**異なる**実装の `IEnumerable<T>` 解決
- [x] ジェネリック型制約**なし**のオープンジェネリック登録（`UseOpenGenericBinding`）
- [x] 実装型・ファクトリーデリゲート・シングルトンインスタンスによる登録
- [x] 明示的なキーによる `[FromKeyedServices]` コンストラクターパラメーター注入
- [x] プロバイダー / スコープの破棄（内部のリゾルバーが破棄される）

## 互換性なし

各項目に、失敗する仕様と、対応に必要な内容を記載します。

### `IServiceProviderIs(Keyed)Service`

- [ ] `IServiceProviderIsService` / `IServiceProviderIsKeyedService` が未登録
  （`Supports…` フラグが `false` のため、これらの仕様はスキップされます）。
  **対応方法:** `SmartServiceProviderFactory.CreateBuilder` でこれらのサービスを、`ResolverConfig` の
  バインディング（`IEnumerable<T>` やオープンジェネリックを含む）を調べて `IsService(Type)` に答える
  実装にバインドする。

### キー付きサービス

キー付きサービスのサポートは部分的です。明示的で一致するキーによる解決は動作しますが、以下のケースは動作しません。

- [ ] **注入された `IServiceProvider` でキー付きサービスを解決できない**
  （`SimpleServiceKeyedResolution`、`ResolveKeyed{Transient,Singleton}FromInjectedServiceProvider`）。
  注入される `IServiceProvider` は素の `SmartResolver` に解決され、`IKeyedServiceProvider` を実装していないため、
  `GetKeyedService` は *"This service provider doesn't support keyed services."* をスローします。
  **対応方法:** `IServiceProvider` / `IKeyedServiceProvider` を、素のリゾルバーではなくアダプターの
  プロバイダー（ルートでは `SmartServiceProvider`、スコープ内では `SmartChildServiceProvider`）にバインドし、
  注入されるプロバイダーがキーおよびスコープを認識できるようにする。

- [ ] **`KeyedService.AnyKey`** の登録とクエリ
  （`ResolveKeyedServicesAnyKey*`、`ResolveKeyedService*WithAnyKey*`、`ResolveWithAnyKeyQuery_*`）。
  キーは等価性で照合されるため、`AnyKey` センチネルはどれにも一致しません。
  **対応方法:** キー付きバインディングの検索で `AnyKey` を特別扱いする。`AnyKey` 登録は任意の要求キーに一致し、
  `AnyKey` クエリはその型のすべてのキー付き登録に一致するようにする。

- [ ] **キー付き `IEnumerable<T>` の解決**
  （`ResolveKeyedServices`、`ResolveKeyedGenericServices`）。
  `UseArrayBinding` は非キー付きバインディングのみを収集するため、`GetKeyedServices<T>(key)` は `null` を返します。
  **対応方法:** 配列バインディングを拡張し、要求キーと一致するキーを持つすべてのバインディングを収集する。

- [ ] **`[ServiceKey]` によるキー注入**
  （`ResolveKeyedServiceSingletonInstanceWithKeyInjection`、`...WithKeyedParameter`、
  `ResolveKeyedServiceWithFromServiceKeyAttribute`、`CreateServiceWithKeyedParameter`）。
  サービスが解決されたときのキーがコンストラクターに注入されません（既定値が使われます）。
  **対応方法:** `[ServiceKey]` を付与したパラメーターに現在の解決キーを供給する `IKeySource` /
  パラメーターソースを追加する（`FromKeyedServicesSource` と同様）。

- [ ] **`null` キーの非キー付き登録へのフォールバック、およびキー付き / 非キー付きの分離**
  （`ResolveNullKeyedService`、`ResolveNonKeyedService`、`CombinationalRegistration`）。
  `null` キーは非キー付きサービスを解決すべきであり、キー付き専用の登録は非キー付き解決に漏れ出すべきではありません。
  **対応方法:** `SmartServiceProvider.GetKeyedService` / `GetRequiredKeyedService` で `serviceKey` が `null` の場合は
  キーなしで解決し、それ以外ではキー付きと非キー付きのバインディングを分離したまま扱う。

- [ ] **`GetRequiredKeyedService` はサービスが見つからない場合にスローする**
  （`ResolveRequiredKeyedServiceThrowsIfNotFound`、`ResolveKeyedServicesAnyKeyConsistency`）。
  現在は `InvalidOperationException` をスローせず `null` を返します。
  **対応方法:** 必須解決のパスで、リゾルバーがインスタンスを返さない場合にスローする。

### ジェネリクス

- [ ] **オープンジェネリックのジェネリック型制約フィルタリング**
  （`ConstrainedOpenGenericServicesCanBeResolved`、`Interface…`、`AbstractClass…`）。
  オープンジェネリック実装の `where T : …` 制約がチェックされないため、満たせないクローズが試行されます。
  **対応方法:** オープンジェネリックをクローズする際、要求された型引数が制約を満たさないバインディングをスキップする。

- [ ] **1 つの `IEnumerable<T>` 内でのオープン + クローズドジェネリックの混在（スロット順序）**
  （`ResolvesMixedOpenClosedGenericsAsEnumerable`、`ResolvingEnumerableContainingOpenGenericServiceUsesCorrectSlot`）。
  クローズドとオープンジェネリックの登録が登録順にマージされません。
  **対応方法:** クローズドとオープンジェネリックの両方のバインディングから、登録順を保ったまま列挙を構築する。

### 列挙のインスタンス化

- [ ] **重複登録は別々のインスタンスを生成する**
  （`ResolvesDifferentInstancesForServiceWhenResolvingEnumerable`）。
  同じ実装を N 回登録すると N 個の要素が生成されるべきですが、配列バインディングは
  それらを 1 つのキャッシュされたインスタンスにまとめてしまいます。
  **対応方法:** シングルトン / スコープのインスタンスをサービス型単位ではなくバインディング単位でキャッシュし、
  各登録が独自の要素を提供するようにする。

### スコープと破棄

- [ ] **注入された `IServiceProvider` が現在のスコープを反映する**
  （`NonSingletonService_WithInjectedProvider_ResolvesScopeProvider`）。
  解決された `IServiceProvider` はルートではなく、アクティブなスコープのプロバイダーであるべきです。
  **対応方法:** `IServiceProvider` をスコープ単位で登録し、子リゾルバーが自身のプロバイダーを注入するようにする
  （上記のキー付き注入プロバイダーの項目と根本原因を共有します）。

- [ ] **コンテナによる生成済みサービスの破棄追跡**
  （`DisposingScopeDisposesService`、`DisposesInReverseOrderOfCreation`、
  `FactoryServicesAreCreatedAsPartOfCreatingObjectGraph`）。
  Transient / ファクトリーが生成した `IDisposable` が追跡されないため、スコープを破棄しても
  そのスコープが生成したサービスは破棄されず、また破棄が生成の逆順で行われません。
  **対応方法:** 生成された各 `IDisposable` / `IAsyncDisposable` を所有スコープで追跡し、
  スコープ / プロバイダーの破棄時に生成の逆順で破棄する。
