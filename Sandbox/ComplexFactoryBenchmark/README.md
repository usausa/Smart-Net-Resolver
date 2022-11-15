``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|       Method |     Mean |     Error |   StdDev |      Min |      Max |      P90 |   Gen0 | Allocated |
|------------- |---------:|----------:|---------:|---------:|---------:|---------:|-------:|----------:|
|      Complex | 79.22 ns |  5.699 ns | 0.312 ns | 79.02 ns | 79.58 ns | 79.47 ns | 0.0081 |     136 B |
| TypedComplex | 70.20 ns | 12.272 ns | 0.673 ns | 69.43 ns | 70.67 ns | 70.63 ns | 0.0081 |     136 B |
