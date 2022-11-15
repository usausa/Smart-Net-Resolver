``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]   : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  ShortRun : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
|               Method |      Mean |     Error |    StdDev |       Min |       Max |       P90 |   Gen0 | Allocated |
|--------------------- |----------:|----------:|----------:|----------:|----------:|----------:|-------:|----------:|
|             Resolver |  4.212 ns | 0.9374 ns | 0.0514 ns |  4.180 ns |  4.271 ns |  4.254 ns | 0.0014 |      24 B |
| ContextScopeResolver | 17.861 ns | 5.5875 ns | 0.3063 ns | 17.682 ns | 18.214 ns | 18.109 ns |      - |         - |
|   ChildScopeResolver | 21.760 ns | 4.5439 ns | 0.2491 ns | 21.509 ns | 22.007 ns | 21.958 ns | 0.0038 |      64 B |
