|          Method |      Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |----------:|----------:|----------:|-------:|------:|------:|----------:|
| DynamicFactory0 |  4.977 ns | 0.0153 ns | 0.0225 ns | 0.0057 |     - |     - |      24 B |
|   ClassFactory0 |  5.884 ns | 0.0263 ns | 0.0393 ns | 0.0057 |     - |     - |      24 B |
| DynamicFactory1 |  8.350 ns | 0.0209 ns | 0.0286 ns | 0.0057 |     - |     - |      24 B |
|   ClassFactory1 |  8.317 ns | 0.0221 ns | 0.0331 ns | 0.0057 |     - |     - |      24 B |
| DynamicFactory2 | 14.648 ns | 0.0484 ns | 0.0725 ns | 0.0133 |     - |     - |      56 B |
|   ClassFactory2 | 14.817 ns | 0.0329 ns | 0.0451 ns | 0.0133 |     - |     - |      56 B |
| DynamicFactory4 | 25.241 ns | 0.0593 ns | 0.0869 ns | 0.0210 |     - |     - |      88 B |
|   ClassFactory4 | 25.750 ns | 0.0636 ns | 0.0932 ns | 0.0210 |     - |     - |      88 B |
