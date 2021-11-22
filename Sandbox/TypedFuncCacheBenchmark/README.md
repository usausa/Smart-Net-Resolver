``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|                      Method |      Mean |     Error |    StdDev |       Min |       Max |       P90 |  Gen 0 | Allocated |
|---------------------------- |----------:|----------:|----------:|----------:|----------:|----------:|-------:|----------:|
| SingletonFromObjectResolver |  5.151 ns | 0.3940 ns | 0.0216 ns |  5.133 ns |  5.175 ns |  5.169 ns |      - |         - |
| TransientFromObjectResolver |  7.740 ns | 0.6969 ns | 0.0382 ns |  7.702 ns |  7.778 ns |  7.770 ns | 0.0014 |      24 B |
|   CombineFromObjectResolver | 14.141 ns | 0.8236 ns | 0.0451 ns | 14.089 ns | 14.170 ns | 14.169 ns | 0.0033 |      56 B |
|   ComplexFromObjectResolver | 35.392 ns | 4.2679 ns | 0.2339 ns | 35.163 ns | 35.631 ns | 35.581 ns | 0.0095 |     160 B |
|  SingletonFromTypedResolver |  6.023 ns | 0.8845 ns | 0.0485 ns |  5.991 ns |  6.079 ns |  6.063 ns |      - |         - |
|  TransientFromTypedResolver |  7.473 ns | 5.1564 ns | 0.2826 ns |  7.296 ns |  7.799 ns |  7.704 ns | 0.0014 |      24 B |
|    CombineFromTypedResolver | 13.141 ns | 1.3667 ns | 0.0749 ns | 13.058 ns | 13.204 ns | 13.195 ns | 0.0033 |      56 B |
|    ComplexFromTypedResolver | 33.695 ns | 3.7999 ns | 0.2083 ns | 33.492 ns | 33.908 ns | 33.864 ns | 0.0095 |     160 B |
