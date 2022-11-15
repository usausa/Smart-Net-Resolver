``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|          Method |      Mean |      Error |    StdDev |       Min |       Max |       P90 |    Gen0 | Allocated |
|---------------- |----------:|-----------:|----------:|----------:|----------:|----------:|--------:|----------:|
| CreateSingleton |  8.507 μs |  0.2924 μs | 0.0160 μs |  8.492 μs |  8.524 μs |  8.520 μs |       - |         - |
| CreateTransient | 12.996 μs |  2.5586 μs | 0.1402 μs | 12.841 μs | 13.114 μs | 13.098 μs |  4.3030 |   72000 B |
|   CreateCombine | 24.841 μs |  3.1113 μs | 0.1705 μs | 24.644 μs | 24.939 μs | 24.939 μs |  8.6060 |  144000 B |
|   CreateComplex | 65.072 μs | 10.6493 μs | 0.5837 μs | 64.408 μs | 65.506 μs | 65.465 μs | 21.4844 |  360000 B |
|  CreateCombine2 | 23.391 μs |  0.6690 μs | 0.0367 μs | 23.349 μs | 23.419 μs | 23.416 μs |  8.6060 |  144000 B |
|  CreateComplex2 | 52.366 μs |  6.4120 μs | 0.3515 μs | 52.045 μs | 52.741 μs | 52.656 μs | 21.4844 |  360000 B |
