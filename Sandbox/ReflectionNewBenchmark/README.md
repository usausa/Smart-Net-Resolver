``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|          Method |       Mean |      Error |    StdDev |        Min |        Max |        P90 |   Gen0 | Allocated |
|---------------- |-----------:|-----------:|----------:|-----------:|-----------:|-----------:|-------:|----------:|
| FuncActivator0A |   8.171 ns |  1.7683 ns | 0.0969 ns |   8.061 ns |   8.245 ns |   8.237 ns | 0.0014 |      24 B |
| FuncActivator0B |   9.010 ns |  1.2972 ns | 0.0711 ns |   8.928 ns |   9.054 ns |   9.053 ns | 0.0014 |      24 B |
| TypedActivator0 |   8.108 ns |  2.5312 ns | 0.1387 ns |   8.020 ns |   8.268 ns |   8.222 ns | 0.0014 |      24 B |
|      Activator0 |   6.992 ns |  0.9597 ns | 0.0526 ns |   6.932 ns |   7.032 ns |   7.028 ns | 0.0014 |      24 B |
|      Activator1 | 138.018 ns | 25.3805 ns | 1.3912 ns | 137.131 ns | 139.621 ns | 139.157 ns | 0.0181 |     304 B |
|      Activator2 | 156.591 ns | 33.3533 ns | 1.8282 ns | 154.770 ns | 158.426 ns | 158.056 ns | 0.0196 |     328 B |
|    Constructor0 |   9.627 ns |  1.1100 ns | 0.0608 ns |   9.557 ns |   9.666 ns |   9.664 ns | 0.0014 |      24 B |
|    Constructor1 |  24.449 ns | 10.4199 ns | 0.5712 ns |  23.946 ns |  25.070 ns |  24.922 ns | 0.0048 |      80 B |
|    Constructor2 |  30.590 ns |  4.5246 ns | 0.2480 ns |  30.312 ns |  30.789 ns |  30.765 ns | 0.0052 |      88 B |
