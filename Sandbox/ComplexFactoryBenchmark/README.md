``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|       Method |     Mean |    Error |   StdDev |      Min |      Max |      P90 |  Gen 0 | Allocated |
|------------- |---------:|---------:|---------:|---------:|---------:|---------:|-------:|----------:|
|      Complex | 96.46 ns | 3.899 ns | 0.214 ns | 96.32 ns | 96.70 ns | 96.63 ns | 0.0081 |     136 B |
| TypedComplex | 70.38 ns | 6.911 ns | 0.379 ns | 70.08 ns | 70.81 ns | 70.70 ns | 0.0081 |     136 B |
