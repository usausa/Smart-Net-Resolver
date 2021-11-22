``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                    Method |     Mean |    Error |   StdDev |      Min |      Max |      P90 |  Gen 0 | Allocated |
|-------------------------- |---------:|---------:|---------:|---------:|---------:|---------:|-------:|----------:|
|      TransientComplexFunc | 47.03 ns | 5.723 ns | 0.314 ns | 46.69 ns | 47.31 ns | 47.26 ns | 0.0167 |     280 B |
| TransientComplexTypedFunc | 72.39 ns | 3.621 ns | 0.198 ns | 72.27 ns | 72.62 ns | 72.56 ns | 0.0167 |     280 B |
