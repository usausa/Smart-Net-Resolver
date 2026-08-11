```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8894/25H2/2025Update/HudsonValley2)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 10.0.302
  [Host]    : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  MediumRun : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3

Job=MediumRun  Jit=RyuJit  Platform=X64  
IterationCount=15  LaunchCount=4  WarmupCount=10  

```
| Method    | Mean     | Error    | StdDev   | Median   | Min      | Max      | P90      | Gen0   | Allocated |
|---------- |---------:|---------:|---------:|---------:|---------:|---------:|---------:|-------:|----------:|
| Transient | 11.38 ns | 1.438 ns | 3.187 ns | 13.37 ns | 7.237 ns | 15.57 ns | 15.13 ns | 0.0011 |      19 B |
