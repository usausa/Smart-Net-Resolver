# Default(DynamicMethod)

## Core

|             Method |  Job | Runtime |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |----- |-------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton | Core |    Core |  25.6769 ns | 0.1779 ns | 0.1486 ns |      - |       0 B |
|          Transient | Core |    Core |  35.8662 ns | 0.4017 ns | 0.3758 ns | 0.0171 |      72 B |
|           Combined | Core |    Core |  49.7437 ns | 0.2083 ns | 0.1949 ns | 0.0171 |      72 B |
|           Generics | Core |    Core |  10.8477 ns | 0.0419 ns | 0.0327 ns | 0.0057 |      24 B |
|  MultipleSingleton | Core |    Core |   8.8937 ns | 0.0306 ns | 0.0271 ns |      - |       0 B |
|  MultipleTransient | Core |    Core |  82.0224 ns | 0.9342 ns | 0.8282 ns | 0.0437 |     184 B |
|         SingletonN | Core |    Core |   1.5870 ns | 0.0071 ns | 0.0055 ns |      - |       0 B |
|         TransientN | Core |    Core |   2.2461 ns | 0.0206 ns | 0.0172 ns | 0.0171 |      72 B |
|          CombinedN | Core |    Core |   3.1003 ns | 0.0100 ns | 0.0093 ns | 0.0171 |      72 B |
|          GenericsN | Core |    Core |   0.6655 ns | 0.0013 ns | 0.0012 ns | 0.0057 |      24 B |
| MultipleSingletonN | Core |    Core |   0.5279 ns | 0.0041 ns | 0.0036 ns |      - |       0 B |
| MultipleTransientN | Core |    Core |   5.1015 ns | 0.0271 ns | 0.0253 ns | 0.0437 |     184 B |

## Clr

|             Method |  Job | Runtime |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |----- |-------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton |  Clr |     Clr |  26.4647 ns | 0.3958 ns | 0.3702 ns |      - |       0 B |
|          Transient |  Clr |     Clr |  52.6530 ns | 2.1053 ns | 3.0193 ns | 0.0171 |      72 B |
|           Combined |  Clr |     Clr |  68.1311 ns | 0.5777 ns | 0.5404 ns | 0.0170 |      72 B |
|           Generics |  Clr |     Clr |  16.6762 ns | 0.3537 ns | 0.4210 ns | 0.0057 |      24 B |
|  MultipleSingleton |  Clr |     Clr |  12.0732 ns | 0.2667 ns | 0.2495 ns |      - |       0 B |
|  MultipleTransient |  Clr |     Clr | 138.1833 ns | 0.9801 ns | 0.9168 ns | 0.0436 |     184 B |
|         SingletonN |  Clr |     Clr |   1.6409 ns | 0.0073 ns | 0.0068 ns |      - |       0 B |
|         TransientN |  Clr |     Clr |   3.1432 ns | 0.0156 ns | 0.0146 ns | 0.0171 |      72 B |
|          CombinedN |  Clr |     Clr |   4.2201 ns | 0.0376 ns | 0.0352 ns | 0.0170 |      72 B |
|          GenericsN |  Clr |     Clr |   1.0175 ns | 0.0039 ns | 0.0037 ns | 0.0057 |      24 B |
| MultipleSingletonN |  Clr |     Clr |   0.7504 ns | 0.0050 ns | 0.0042 ns |      - |       0 B |
| MultipleTransientN |  Clr |     Clr |   8.7552 ns | 0.0562 ns | 0.0470 ns | 0.0436 |     184 B |

# Emit(Reference)

## Core

|             Method |  Job | Runtime |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |----- |-------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton | Core |    Core |  25.6106 ns | 0.0792 ns | 0.0741 ns |      - |       0 B |
|          Transient | Core |    Core |  36.2624 ns | 0.2032 ns | 0.1901 ns | 0.0171 |      72 B |
|           Combined | Core |    Core |  53.1066 ns | 0.2796 ns | 0.2479 ns | 0.0171 |      72 B |
|           Generics | Core |    Core |  11.6061 ns | 0.0547 ns | 0.0511 ns | 0.0057 |      24 B |
|  MultipleSingleton | Core |    Core |   8.8123 ns | 0.0229 ns | 0.0214 ns |      - |       0 B |
|  MultipleTransient | Core |    Core |  86.6145 ns | 0.3143 ns | 0.2940 ns | 0.0437 |     184 B |
|         SingletonN | Core |    Core |   1.6015 ns | 0.0067 ns | 0.0056 ns |      - |       0 B |
|         TransientN | Core |    Core |   2.2677 ns | 0.0027 ns | 0.0022 ns | 0.0171 |      72 B |
|          CombinedN | Core |    Core |   3.2894 ns | 0.0271 ns | 0.0253 ns | 0.0171 |      72 B |
|          GenericsN | Core |    Core |   0.7503 ns | 0.0168 ns | 0.0295 ns | 0.0057 |      24 B |
| MultipleSingletonN | Core |    Core |   0.5336 ns | 0.0035 ns | 0.0033 ns |      - |       0 B |
| MultipleTransientN | Core |    Core |   5.4375 ns | 0.0270 ns | 0.0253 ns | 0.0437 |     184 B |

## Clr

|             Method |  Job | Runtime |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |----- |-------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton |  Clr |     Clr |  23.6960 ns | 0.2874 ns | 0.2689 ns |      - |       0 B |
|          Transient |  Clr |     Clr |  30.3582 ns | 0.1242 ns | 0.1101 ns | 0.0171 |      72 B |
|           Combined |  Clr |     Clr |  52.4517 ns | 0.1491 ns | 0.1395 ns | 0.0171 |      72 B |
|           Generics |  Clr |     Clr |   9.4905 ns | 0.0440 ns | 0.0390 ns | 0.0057 |      24 B |
|  MultipleSingleton |  Clr |     Clr |  10.4268 ns | 0.0576 ns | 0.0539 ns |      - |       0 B |
|  MultipleTransient |  Clr |     Clr | 105.5245 ns | 0.9103 ns | 0.8069 ns | 0.0437 |     184 B |
|         SingletonN |  Clr |     Clr |   1.4670 ns | 0.0067 ns | 0.0052 ns |      - |       0 B |
|         TransientN |  Clr |     Clr |   1.8874 ns | 0.0082 ns | 0.0076 ns | 0.0171 |      72 B |
|          CombinedN |  Clr |     Clr |   3.0126 ns | 0.0096 ns | 0.0089 ns | 0.0171 |      72 B |
|          GenericsN |  Clr |     Clr |   0.5969 ns | 0.0022 ns | 0.0020 ns | 0.0057 |      24 B |
| MultipleSingletonN |  Clr |     Clr |   0.6570 ns | 0.0021 ns | 0.0017 ns |      - |       0 B |
| MultipleTransientN |  Clr |     Clr |   6.6197 ns | 0.0269 ns | 0.0251 ns | 0.0437 |     184 B |

# Reflection

## Core

|             Method |  Job | Runtime |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |----- |-------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton | Core |    Core |  25.7503 ns | 0.1283 ns | 0.1071 ns |      - |       0 B |
|          Transient | Core |    Core | 425.3526 ns | 2.0369 ns | 1.9053 ns | 0.0167 |      72 B |
|           Combined | Core |    Core | 602.8689 ns | 2.4087 ns | 2.2531 ns | 0.0620 |     264 B |
|           Generics | Core |    Core | 123.7731 ns | 0.4329 ns | 0.4050 ns | 0.0055 |      24 B |
|  MultipleSingleton | Core |    Core |   8.5765 ns | 0.0579 ns | 0.0542 ns |      - |       0 B |
|  MultipleTransient | Core |    Core | 559.4936 ns | 2.2371 ns | 2.0926 ns | 0.0429 |     184 B |
|         SingletonN | Core |    Core |   1.6084 ns | 0.0064 ns | 0.0060 ns |      - |       0 B |
|         TransientN | Core |    Core |  26.9610 ns | 0.0779 ns | 0.0728 ns | 0.0167 |      72 B |
|          CombinedN | Core |    Core |  37.1998 ns | 0.1561 ns | 0.1460 ns | 0.0620 |     264 B |
|          GenericsN | Core |    Core |   7.8233 ns | 0.0297 ns | 0.0278 ns | 0.0055 |      24 B |
| MultipleSingletonN | Core |    Core |   0.5360 ns | 0.0026 ns | 0.0024 ns |      - |       0 B |
| MultipleTransientN | Core |    Core |  34.5840 ns | 0.0517 ns | 0.0484 ns | 0.0429 |     184 B |

## Clr

|             Method |  Job | Runtime |        Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------- |----- |-------- |------------:|----------:|----------:|-------:|----------:|
|          Singleton |  Clr |     Clr |  27.5879 ns | 0.0891 ns | 0.0790 ns |      - |       0 B |
|          Transient |  Clr |     Clr | 276.5685 ns | 1.6678 ns | 1.5601 ns | 0.0167 |      72 B |
|           Combined |  Clr |     Clr | 694.4202 ns | 2.7652 ns | 2.5866 ns | 0.0620 |     264 B |
|           Generics |  Clr |     Clr |  75.6849 ns | 0.2532 ns | 0.2368 ns | 0.0056 |      24 B |
|  MultipleSingleton |  Clr |     Clr |   8.7305 ns | 0.0230 ns | 0.0215 ns |      - |       0 B |
|  MultipleTransient |  Clr |     Clr | 517.0698 ns | 1.3339 ns | 1.1825 ns | 0.0429 |     184 B |
|         SingletonN |  Clr |     Clr |   1.6792 ns | 0.0030 ns | 0.0025 ns |      - |       0 B |
|         TransientN |  Clr |     Clr |  17.4039 ns | 0.0299 ns | 0.0250 ns | 0.0167 |      72 B |
|          CombinedN |  Clr |     Clr |  43.2819 ns | 0.0530 ns | 0.0383 ns | 0.0620 |     264 B |
|          GenericsN |  Clr |     Clr |   4.7772 ns | 0.0166 ns | 0.0147 ns | 0.0056 |      24 B |
| MultipleSingletonN |  Clr |     Clr |   0.5461 ns | 0.0014 ns | 0.0013 ns |      - |       0 B |
| MultipleTransientN |  Clr |     Clr |  32.3177 ns | 0.1107 ns | 0.1035 ns | 0.0429 |     184 B |
