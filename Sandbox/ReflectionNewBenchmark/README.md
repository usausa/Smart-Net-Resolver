``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.253 (1809/October2018Update/Redstone5)
Intel Core i7-4771 CPU 3.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview4-011223
  [Host]   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  ShortRun : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|          Method |      Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |----------:|----------:|----------:|-------:|------:|------:|----------:|
| TypedActivator0 |  49.16 ns |  6.103 ns | 0.3345 ns | 0.0057 |     - |     - |      24 B |
|      Activator0 |  41.92 ns |  7.193 ns | 0.3943 ns | 0.0057 |     - |     - |      24 B |
|      Activator1 | 590.41 ns | 37.000 ns | 2.0281 ns | 0.1040 |     - |     - |     440 B |
|      Activator2 | 695.11 ns | 74.867 ns | 4.1037 ns | 0.1116 |     - |     - |     472 B |
|    Constructor0 | 102.05 ns | 17.121 ns | 0.9385 ns | 0.0056 |     - |     - |      24 B |
|    Constructor1 | 191.41 ns | 33.878 ns | 1.8570 ns | 0.0265 |     - |     - |     112 B |
|    Constructor2 | 245.79 ns | 45.945 ns | 2.5184 ns | 0.0305 |     - |     - |     128 B |
