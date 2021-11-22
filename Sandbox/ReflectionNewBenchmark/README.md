``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|          Method |       Mean |      Error |    StdDev |        Min |        Max |        P90 |  Gen 0 | Allocated |
|---------------- |-----------:|-----------:|----------:|-----------:|-----------:|-----------:|-------:|----------:|
| FuncActivator0A |   7.382 ns |  0.6400 ns | 0.0351 ns |   7.347 ns |   7.417 ns |   7.410 ns | 0.0014 |      24 B |
| FuncActivator0B |   7.907 ns |  2.3712 ns | 0.1300 ns |   7.793 ns |   8.049 ns |   8.015 ns | 0.0014 |      24 B |
| TypedActivator0 |   7.166 ns |  2.5455 ns | 0.1395 ns |   7.069 ns |   7.326 ns |   7.282 ns | 0.0014 |      24 B |
|      Activator0 |   6.070 ns |  2.0752 ns | 0.1138 ns |   5.987 ns |   6.200 ns |   6.165 ns | 0.0014 |      24 B |
|      Activator1 | 238.626 ns |  2.7008 ns | 0.1480 ns | 238.475 ns | 238.771 ns | 238.743 ns | 0.0238 |     400 B |
|      Activator2 | 271.752 ns | 12.1087 ns | 0.6637 ns | 271.173 ns | 272.477 ns | 272.302 ns | 0.0253 |     424 B |
|    Constructor0 |  42.455 ns | 21.6440 ns | 1.1864 ns |  41.764 ns |  43.825 ns |  43.415 ns | 0.0014 |      24 B |
|    Constructor1 |  68.994 ns |  2.5409 ns | 0.1393 ns |  68.868 ns |  69.143 ns |  69.109 ns | 0.0048 |      80 B |
|    Constructor2 |  90.266 ns |  1.9737 ns | 0.1082 ns |  90.179 ns |  90.387 ns |  90.356 ns | 0.0052 |      88 B |
