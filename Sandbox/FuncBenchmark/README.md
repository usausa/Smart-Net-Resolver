``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|        Method |      Mean |      Error |    StdDev |       Min |       Max |       P90 |  Gen 0 | Allocated |
|-------------- |----------:|-----------:|----------:|----------:|----------:|----------:|-------:|----------:|
|    NonSealed0 |  2.896 ns |  0.5207 ns | 0.0285 ns |  2.869 ns |  2.926 ns |  2.919 ns | 0.0014 |      24 B |
|    NonSealed1 |  6.872 ns |  1.3071 ns | 0.0716 ns |  6.790 ns |  6.920 ns |  6.917 ns | 0.0029 |      48 B |
|    NonSealed2 | 11.305 ns |  1.2989 ns | 0.0712 ns | 11.253 ns | 11.386 ns | 11.364 ns | 0.0048 |      80 B |
|       Sealed0 |  2.896 ns |  0.3849 ns | 0.0211 ns |  2.873 ns |  2.914 ns |  2.912 ns | 0.0014 |      24 B |
|       Sealed1 |  6.872 ns |  0.4883 ns | 0.0268 ns |  6.841 ns |  6.888 ns |  6.888 ns | 0.0029 |      48 B |
|       Sealed2 | 11.269 ns |  2.9751 ns | 0.1631 ns | 11.081 ns | 11.372 ns | 11.368 ns | 0.0048 |      80 B |
| NonInterface0 |  2.889 ns |  0.0310 ns | 0.0017 ns |  2.887 ns |  2.891 ns |  2.890 ns | 0.0014 |      24 B |
| NonInterface1 |  6.612 ns |  1.9419 ns | 0.1064 ns |  6.521 ns |  6.729 ns |  6.700 ns | 0.0029 |      48 B |
| NonInterface2 | 11.111 ns |  1.7631 ns | 0.0966 ns | 11.001 ns | 11.182 ns | 11.175 ns | 0.0048 |      80 B |
|       Direct0 |  2.895 ns |  0.5753 ns | 0.0315 ns |  2.860 ns |  2.922 ns |  2.918 ns | 0.0014 |      24 B |
|       Direct1 |  4.143 ns |  0.2548 ns | 0.0140 ns |  4.127 ns |  4.152 ns |  4.152 ns | 0.0014 |      24 B |
|       Direct2 |  5.384 ns |  0.2752 ns | 0.0151 ns |  5.367 ns |  5.395 ns |  5.394 ns | 0.0019 |      32 B |
|      Direct01 |  2.920 ns |  0.1743 ns | 0.0096 ns |  2.911 ns |  2.930 ns |  2.927 ns | 0.0014 |      24 B |
|      Direct11 |  4.452 ns |  0.0404 ns | 0.0022 ns |  4.450 ns |  4.454 ns |  4.454 ns | 0.0014 |      24 B |
|      Direct21 |  6.199 ns | 13.3268 ns | 0.7305 ns |  5.719 ns |  7.040 ns |  6.800 ns | 0.0019 |      32 B |
