``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=6.0.100
  [Host]   : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  ShortRun : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|               Method |      Mean |    Error |    StdDev |       Min |       Max |       P90 |  Gen 0 | Allocated |
|--------------------- |----------:|---------:|----------:|----------:|----------:|----------:|-------:|----------:|
|             Resolver |  4.091 ns | 1.069 ns | 0.0586 ns |  4.056 ns |  4.158 ns |  4.138 ns | 0.0014 |      24 B |
| ContextScopeResolver | 15.939 ns | 1.404 ns | 0.0769 ns | 15.881 ns | 16.026 ns | 16.002 ns |      - |         - |
|   ChildScopeResolver | 20.654 ns | 1.533 ns | 0.0841 ns | 20.569 ns | 20.737 ns | 20.721 ns | 0.0038 |      64 B |
