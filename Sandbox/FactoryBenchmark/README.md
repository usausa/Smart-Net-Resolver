``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|   Method |     Mean |     Error |   StdDev |      Min |      Max |      P90 |  Gen 0 | Allocated |
|--------- |---------:|----------:|---------:|---------:|---------:|---------:|-------:|----------:|
| Factory1 | 72.83 ns | 16.484 ns | 0.904 ns | 72.11 ns | 73.84 ns | 73.58 ns | 0.0205 |     344 B |
| Factory2 | 37.15 ns |  4.692 ns | 0.257 ns | 36.91 ns | 37.42 ns | 37.36 ns | 0.0095 |     160 B |
| Factory3 | 31.91 ns |  2.646 ns | 0.145 ns | 31.75 ns | 32.02 ns | 32.01 ns | 0.0095 |     160 B |
| Factory4 | 30.39 ns |  0.345 ns | 0.019 ns | 30.37 ns | 30.40 ns | 30.40 ns | 0.0095 |     160 B |
