# Default(DynamicMethod)

## Core

|            Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|----------:|
|         Singleton |  8.623 ns | 0.1140 ns | 0.1066 ns |      - |       0 B |
|         Transient | 11.521 ns | 0.1563 ns | 0.1462 ns | 0.0285 |     120 B |
|          Combined | 19.268 ns | 0.2529 ns | 0.2366 ns | 0.0285 |     120 B |
|           Complex | 76.515 ns | 0.7056 ns | 0.6255 ns | 0.0323 |     136 B |
|          Generics | 11.438 ns | 0.1625 ns | 0.1520 ns | 0.0057 |      24 B |
| MultipleSingleton |  8.574 ns | 0.1211 ns | 0.1074 ns |      - |       0 B |
| MultipleTransient | 90.122 ns | 1.3650 ns | 1.2769 ns | 0.0437 |     184 B |

# Reflection

## Core

|            Method |         Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |-------------:|----------:|----------:|-------:|----------:|
|         Singleton |     8.695 ns | 0.3449 ns | 0.4605 ns |      - |       0 B |
|         Transient |    74.876 ns | 0.8357 ns | 0.7409 ns | 0.0281 |     120 B |
|          Combined |   203.803 ns | 3.5602 ns | 3.3302 ns | 0.1030 |     440 B |
|           Complex | 1,063.403 ns | 7.9425 ns | 7.4294 ns | 0.1106 |     472 B |
|          Generics |    87.447 ns | 1.0011 ns | 0.9364 ns | 0.0056 |      24 B |
| MultipleSingleton |     8.582 ns | 0.1243 ns | 0.1162 ns |      - |       0 B |
| MultipleTransient |   519.122 ns | 8.7496 ns | 8.1843 ns | 0.0429 |     184 B |
