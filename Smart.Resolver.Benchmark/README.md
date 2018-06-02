# Default(DynamicMethod)

## Core

|             Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |-----------:|----------:|----------:|-------:|----------:|
|          Singleton | 24.8291 ns | 1.1030 ns | 1.1802 ns |      - |       0 B |
|          Transient | 33.7850 ns | 0.3104 ns | 0.2751 ns | 0.0171 |      72 B |
|           Combined | 53.7093 ns | 0.5083 ns | 0.4245 ns | 0.0171 |      72 B |
|           Generics | 11.0496 ns | 0.0684 ns | 0.0640 ns | 0.0057 |      24 B |
|  MultipleSingleton | 12.2244 ns | 0.1647 ns | 0.1286 ns |      - |       0 B |
|  MultipleTransient | 91.3802 ns | 0.7694 ns | 0.6821 ns | 0.0437 |     184 B |
|         SingletonN |  1.5507 ns | 0.0122 ns | 0.0114 ns |      - |       0 B |
|         TransientN |  2.0198 ns | 0.0187 ns | 0.0175 ns | 0.0171 |      72 B |
|          CombinedN |  3.3736 ns | 0.0303 ns | 0.0253 ns | 0.0171 |      72 B |
|          GenericsN |  0.6949 ns | 0.0101 ns | 0.0090 ns | 0.0057 |      24 B |
| MultipleSingletonN |  0.7681 ns | 0.0104 ns | 0.0092 ns |      - |       0 B |
| MultipleTransientN |  5.5571 ns | 0.0215 ns | 0.0191 ns | 0.0437 |     184 B |

# Reflection

## Core

|             Method |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton |  24.5323 ns | 0.1795 ns | 0.1499 ns |      - |       0 B |
|          Transient | 304.5886 ns | 2.3628 ns | 2.0946 ns | 0.0167 |      72 B |
|           Combined | 606.6777 ns | 3.6148 ns | 3.3813 ns | 0.0620 |     264 B |
|           Generics |  85.9913 ns | 0.8807 ns | 0.7807 ns | 0.0056 |      24 B |
|  MultipleSingleton |  12.6163 ns | 0.0878 ns | 0.0821 ns |      - |       0 B |
|  MultipleTransient | 508.3925 ns | 4.3877 ns | 3.8895 ns | 0.0429 |     184 B |
|         SingletonN |   1.5693 ns | 0.0090 ns | 0.0084 ns |      - |       0 B |
|         TransientN |  19.0295 ns | 0.2387 ns | 0.2233 ns | 0.0167 |      72 B |
|          CombinedN |  37.3789 ns | 0.2262 ns | 0.2116 ns | 0.0620 |     264 B |
|          GenericsN |   5.2641 ns | 0.0564 ns | 0.0528 ns | 0.0056 |      24 B |
| MultipleSingletonN |   0.7715 ns | 0.0052 ns | 0.0048 ns |      - |       0 B |
| MultipleTransientN |  31.8141 ns | 0.3150 ns | 0.2630 ns | 0.0429 |     184 B |
