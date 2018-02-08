# Default(DynamicMethod) - netcoreapp2.0

|            Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|----------:|
|         Singleton | 29.049 ns | 0.2909 ns | 0.4353 ns |      - |       0 B |
|         Transient | 40.422 ns | 0.1546 ns | 0.2266 ns | 0.0171 |      72 B |
|          Combined | 57.514 ns | 0.2349 ns | 0.3516 ns | 0.0171 |      72 B |
|          Generics | 12.219 ns | 0.0541 ns | 0.0776 ns | 0.0057 |      24 B |
|  MultipleSinglton |  9.645 ns | 0.0435 ns | 0.0638 ns |      - |       0 B |
| MultipleTransient | 92.915 ns | 0.4178 ns | 0.5992 ns | 0.0437 |     184 B |

# Emit(Reference) - netcoreapp2.0

|            Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|----------:|
|         Singleton | 27.037 ns | 0.9125 ns | 1.3375 ns |      - |       0 B |
|         Transient | 38.500 ns | 0.1759 ns | 0.2522 ns | 0.0171 |      72 B |
|          Combined | 56.009 ns | 0.3304 ns | 0.4946 ns | 0.0171 |      72 B |
|          Generics | 11.711 ns | 0.0775 ns | 0.1135 ns | 0.0057 |      24 B |
|  MultipleSinglton |  9.009 ns | 0.0546 ns | 0.0817 ns |      - |       0 B |
| MultipleTransient | 96.954 ns | 0.5658 ns | 0.7932 ns | 0.0437 |     184 B |

# Reflection - netcoreapp2.0

|            Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |-----------:|----------:|----------:|-------:|----------:|
|         Singleton |  25.987 ns | 0.1790 ns | 0.2567 ns |      - |       0 B |
|         Transient | 470.319 ns | 2.4451 ns | 3.5840 ns | 0.0167 |      72 B |
|          Combined | 672.911 ns | 4.7952 ns | 7.0288 ns | 0.0620 |     264 B |
|          Generics | 135.898 ns | 1.1724 ns | 1.7548 ns | 0.0055 |      24 B |
|  MultipleSinglton |   8.617 ns | 0.0588 ns | 0.0825 ns |      - |       0 B |
| MultipleTransient | 631.914 ns | 5.5944 ns | 8.0234 ns | 0.0429 |     184 B |

# Default(DynamicMethod) - net47

|            Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|----------:|
|         Singleton |  25.11 ns | 0.1296 ns | 0.1899 ns |      - |       0 B |
|         Transient |  52.73 ns | 0.2787 ns | 0.4085 ns | 0.0171 |      72 B |
|          Combined |  69.86 ns | 0.5692 ns | 0.8519 ns | 0.0170 |      72 B |
|          Generics |  17.07 ns | 0.1609 ns | 0.2408 ns | 0.0057 |      24 B |
|  MultipleSinglton |  11.48 ns | 0.0532 ns | 0.0779 ns |      - |       0 B |
| MultipleTransient | 154.84 ns | 1.0041 ns | 1.5029 ns | 0.0436 |     184 B |

# Emit(Reference) - net47

|            Method |      Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|----------:|
|         Singleton |  28.84 ns | 0.1494 ns | 0.2094 ns |      - |       0 B |
|         Transient |  36.57 ns | 0.2163 ns | 0.3171 ns | 0.0171 |      72 B |
|          Combined |  55.21 ns | 0.2928 ns | 0.4104 ns | 0.0171 |      72 B |
|          Generics |  12.02 ns | 0.0730 ns | 0.1047 ns | 0.0057 |      24 B |
|  MultipleSinglton |  13.00 ns | 0.0760 ns | 0.1114 ns |      - |       0 B |
| MultipleTransient | 128.73 ns | 1.0345 ns | 1.5164 ns | 0.0436 |     184 B |
