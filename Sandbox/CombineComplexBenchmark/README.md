``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|          Method |      Mean |     Error |    StdDev |       Min |       Max |       P90 |   Gen 0 | Allocated |
|---------------- |----------:|----------:|----------:|----------:|----------:|----------:|--------:|----------:|
| CreateSingleton |  8.376 μs | 1.2802 μs | 0.0702 μs |  8.332 μs |  8.457 μs |  8.434 μs |       - |         - |
| CreateTransient | 13.298 μs | 1.0211 μs | 0.0560 μs | 13.252 μs | 13.360 μs | 13.345 μs |  4.3030 |  72,000 B |
|   CreateCombine | 28.173 μs | 0.7504 μs | 0.0411 μs | 28.129 μs | 28.211 μs | 28.205 μs |  8.6060 | 144,000 B |
|   CreateComplex | 67.771 μs | 3.7486 μs | 0.2055 μs | 67.575 μs | 67.985 μs | 67.938 μs | 21.4844 | 360,000 B |
|  CreateCombine2 | 24.992 μs | 1.1296 μs | 0.0619 μs | 24.921 μs | 25.034 μs | 25.031 μs |  8.6060 | 144,000 B |
|  CreateComplex2 | 51.917 μs | 6.5058 μs | 0.3566 μs | 51.635 μs | 52.318 μs | 52.214 μs | 21.4844 | 360,000 B |
