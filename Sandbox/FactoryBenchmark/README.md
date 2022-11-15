``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|   Method |     Mean |     Error |   StdDev |      Min |      Max |      P90 |   Gen0 | Allocated |
|--------- |---------:|----------:|---------:|---------:|---------:|---------:|-------:|----------:|
| Factory1 | 68.31 ns | 34.349 ns | 1.883 ns | 67.01 ns | 70.47 ns | 69.86 ns | 0.0205 |     344 B |
| Factory2 | 35.95 ns |  6.230 ns | 0.342 ns | 35.62 ns | 36.30 ns | 36.23 ns | 0.0095 |     160 B |
| Factory3 | 30.01 ns |  3.306 ns | 0.181 ns | 29.90 ns | 30.22 ns | 30.16 ns | 0.0095 |     160 B |
| Factory4 | 29.32 ns |  5.645 ns | 0.309 ns | 28.96 ns | 29.54 ns | 29.52 ns | 0.0095 |     160 B |
