``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                    Method |     Mean |     Error |   StdDev |      Min |      Max |      P90 |   Gen0 | Allocated |
|-------------------------- |---------:|----------:|---------:|---------:|---------:|---------:|-------:|----------:|
|      TransientComplexFunc | 45.36 ns |  8.882 ns | 0.487 ns | 44.85 ns | 45.82 ns | 45.74 ns | 0.0167 |     280 B |
| TransientComplexTypedFunc | 74.52 ns | 36.669 ns | 2.010 ns | 72.80 ns | 76.73 ns | 76.19 ns | 0.0167 |     280 B |
