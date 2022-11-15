``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.755)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2


```
|     Method |      Mean |     Error |    StdDev |
|----------- |----------:|----------:|----------:|
| Interface0 |  2.924 ns | 0.0254 ns | 0.0237 ns |
| Interface1 |  7.004 ns | 0.0408 ns | 0.0362 ns |
| Interface2 | 11.041 ns | 0.1118 ns | 0.0933 ns |
| Interface4 | 18.573 ns | 0.1211 ns | 0.1133 ns |
| InterfaceX | 48.472 ns | 0.5437 ns | 0.5086 ns |
|      Func0 |  3.283 ns | 0.0710 ns | 0.2094 ns |
|      Func1 |  6.535 ns | 0.0624 ns | 0.0583 ns |
|      Func2 | 11.210 ns | 0.1510 ns | 0.1261 ns |
|      Func4 | 18.413 ns | 0.1467 ns | 0.1301 ns |
|      FuncX | 49.843 ns | 0.4607 ns | 0.4084 ns |
