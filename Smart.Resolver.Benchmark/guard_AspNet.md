```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8894/25H2/2025Update/HudsonValley2)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 10.0.302
  [Host]    : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  MediumRun : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3

Job=MediumRun  Jit=RyuJit  Platform=X64  
IterationCount=15  LaunchCount=4  WarmupCount=10  

```
| Method | Mean     | Error    | StdDev   | Min      | Max      | P90      | Gen0   | Allocated |
|------- |---------:|---------:|---------:|---------:|---------:|---------:|-------:|----------:|
| AspNet | 78.67 ns | 1.430 ns | 3.199 ns | 73.65 ns | 85.27 ns | 83.56 ns | 0.0138 |     232 B |
