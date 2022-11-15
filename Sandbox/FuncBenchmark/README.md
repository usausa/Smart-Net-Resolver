``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|        Method |      Mean |     Error |    StdDev |       Min |       Max |       P90 |   Gen0 | Allocated |
|-------------- |----------:|----------:|----------:|----------:|----------:|----------:|-------:|----------:|
|    NonSealed0 |  2.994 ns | 0.7346 ns | 0.0403 ns |  2.950 ns |  3.029 ns |  3.024 ns | 0.0014 |      24 B |
|    NonSealed1 |  6.734 ns | 2.2918 ns | 0.1256 ns |  6.590 ns |  6.822 ns |  6.815 ns | 0.0029 |      48 B |
|    NonSealed2 | 10.383 ns | 2.2585 ns | 0.1238 ns | 10.262 ns | 10.509 ns | 10.483 ns | 0.0048 |      80 B |
|       Sealed0 |  2.980 ns | 0.6793 ns | 0.0372 ns |  2.938 ns |  3.010 ns |  3.006 ns | 0.0014 |      24 B |
|       Sealed1 |  6.683 ns | 1.1908 ns | 0.0653 ns |  6.614 ns |  6.743 ns |  6.733 ns | 0.0029 |      48 B |
|       Sealed2 | 10.204 ns | 0.4100 ns | 0.0225 ns | 10.178 ns | 10.218 ns | 10.217 ns | 0.0048 |      80 B |
| NonInterface0 |  2.925 ns | 0.3594 ns | 0.0197 ns |  2.912 ns |  2.948 ns |  2.941 ns | 0.0014 |      24 B |
| NonInterface1 |  6.642 ns | 0.8482 ns | 0.0465 ns |  6.589 ns |  6.673 ns |  6.671 ns | 0.0029 |      48 B |
| NonInterface2 | 10.596 ns | 5.1452 ns | 0.2820 ns | 10.408 ns | 10.921 ns | 10.828 ns | 0.0048 |      80 B |
|       Direct0 |  2.977 ns | 0.6580 ns | 0.0361 ns |  2.935 ns |  3.002 ns |  3.000 ns | 0.0014 |      24 B |
|       Direct1 |  4.665 ns | 0.6265 ns | 0.0343 ns |  4.634 ns |  4.702 ns |  4.693 ns | 0.0014 |      24 B |
|       Direct2 |  4.931 ns | 0.5409 ns | 0.0296 ns |  4.907 ns |  4.964 ns |  4.956 ns | 0.0019 |      32 B |
|      Direct01 |  2.938 ns | 0.3640 ns | 0.0199 ns |  2.925 ns |  2.961 ns |  2.954 ns | 0.0014 |      24 B |
|      Direct11 |  4.712 ns | 2.8327 ns | 0.1553 ns |  4.573 ns |  4.879 ns |  4.840 ns | 0.0014 |      24 B |
|      Direct21 |  4.951 ns | 0.8494 ns | 0.0466 ns |  4.898 ns |  4.986 ns |  4.982 ns | 0.0019 |      32 B |
