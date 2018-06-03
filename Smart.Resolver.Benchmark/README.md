# Default(DynamicMethod)

## Core

|            Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|----------:|
|         Singleton |  8.541 ns | 0.0941 ns | 0.0880 ns |      - |       0 B |
|         Transient | 11.672 ns | 0.0996 ns | 0.0932 ns | 0.0286 |     120 B |
|          Combined | 19.053 ns | 0.1502 ns | 0.1405 ns | 0.0285 |     120 B |
|           Complex | 76.915 ns | 0.3628 ns | 0.3216 ns | 0.1616 |     680 B |
|          Generics | 11.433 ns | 0.0911 ns | 0.0852 ns | 0.0285 |     120 B |
| MultipleSingleton |  8.551 ns | 0.0453 ns | 0.0424 ns |      - |       0 B |
| MultipleTransient | 88.916 ns | 0.8927 ns | 0.7454 ns | 0.2189 |     920 B |

# Reflection

## Core

|            Method |         Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |-------------:|----------:|----------:|-------:|----------:|
|         Singleton |     8.528 ns | 0.0682 ns | 0.0638 ns |      - |       0 B |
|         Transient |    74.631 ns | 0.6611 ns | 0.5521 ns | 0.0281 |     120 B |
|          Combined |   203.770 ns | 1.1394 ns | 1.0100 ns | 0.1030 |     440 B |
|           Complex | 1,053.175 ns | 8.0142 ns | 7.4965 ns | 0.5569 |    2360 B |
|          Generics |    89.974 ns | 0.7538 ns | 0.7051 ns | 0.0281 |     120 B |
| MultipleSingleton |     8.461 ns | 0.0683 ns | 0.0605 ns |      - |       0 B |
| MultipleTransient |   514.246 ns | 4.8931 ns | 4.3376 ns | 0.2174 |     920 B |
