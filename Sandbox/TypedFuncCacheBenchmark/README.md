``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                      Method |      Mean |      Error |    StdDev |       Min |       Max |       P90 |   Gen0 | Allocated |
|---------------------------- |----------:|-----------:|----------:|----------:|----------:|----------:|-------:|----------:|
| SingletonFromObjectResolver |  6.481 ns |  0.1898 ns | 0.0104 ns |  6.469 ns |  6.489 ns |  6.488 ns |      - |         - |
| TransientFromObjectResolver |  9.433 ns |  1.1826 ns | 0.0648 ns |  9.360 ns |  9.486 ns |  9.479 ns | 0.0014 |      24 B |
|   CombineFromObjectResolver | 15.186 ns |  1.4606 ns | 0.0801 ns | 15.098 ns | 15.254 ns | 15.244 ns | 0.0033 |      56 B |
|   ComplexFromObjectResolver | 34.401 ns |  4.3227 ns | 0.2369 ns | 34.144 ns | 34.611 ns | 34.578 ns | 0.0095 |     160 B |
|  SingletonFromTypedResolver |  7.577 ns |  0.0862 ns | 0.0047 ns |  7.572 ns |  7.582 ns |  7.581 ns |      - |         - |
|  TransientFromTypedResolver |  8.795 ns |  5.3068 ns | 0.2909 ns |  8.479 ns |  9.051 ns |  9.012 ns | 0.0014 |      24 B |
|    CombineFromTypedResolver | 16.626 ns | 18.0259 ns | 0.9881 ns | 15.756 ns | 17.701 ns | 17.445 ns | 0.0033 |      56 B |
|    ComplexFromTypedResolver | 31.440 ns |  4.8467 ns | 0.2657 ns | 31.140 ns | 31.645 ns | 31.623 ns | 0.0095 |     160 B |
