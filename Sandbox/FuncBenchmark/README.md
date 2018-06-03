``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-4771 CPU 3.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=3415997 Hz, Resolution=292.7403 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]   : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  ShortRun : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT

Job=ShortRun  LaunchCount=1  TargetCount=3  
WarmupCount=3  

```
|        Method |      Mean |      Error |    StdDev |      Gen 0 | Allocated |
|-------------- |----------:|-----------:|----------:|-----------:|----------:|
|    NonSealed0 |  6.911 ns |  1.6959 ns | 0.0958 ns |  5718.7500 |  22.89 MB |
|    NonSealed1 | 16.651 ns | 30.1891 ns | 1.7057 ns | 11437.5000 |  45.78 MB |
|    NonSealed2 | 18.113 ns |  3.6969 ns | 0.2089 ns | 19062.5000 |  76.29 MB |
|       Sealed0 |  5.574 ns |  1.9767 ns | 0.1117 ns |  5718.7500 |  22.89 MB |
|       Sealed1 | 11.728 ns |  0.4904 ns | 0.0277 ns | 11437.5000 |  45.78 MB |
|       Sealed2 | 18.009 ns |  1.9924 ns | 0.1126 ns | 19062.5000 |  76.29 MB |
| NonInterface0 |  5.423 ns |  1.3549 ns | 0.0766 ns |  5718.7500 |  22.89 MB |
| NonInterface1 | 11.959 ns |  4.6328 ns | 0.2618 ns | 11437.5000 |  45.78 MB |
| NonInterface2 | 17.895 ns |  3.1461 ns | 0.1778 ns | 19062.5000 |  76.29 MB |
|       Direct0 |  5.348 ns |  0.7068 ns | 0.0399 ns |  5718.7500 |  22.89 MB |
|       Direct1 |  7.321 ns |  0.2845 ns | 0.0161 ns |  5718.7500 |  22.89 MB |
|       Direct2 |  9.369 ns |  0.6012 ns | 0.0340 ns |  7625.0000 |  30.52 MB |
