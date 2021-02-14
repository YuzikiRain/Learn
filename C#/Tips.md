### 使用非int32类型的枚举

``` csharp
enum ColorType : byte { None, Light = 0b00000001, Dark = 0b00000010, Custom = 0b00000100, }
```

### 直接使用二进制/十六进制表示初始值

``` csharp
// 二进制
byte a = 0b00000001;
// 十六进制
int b = 0x123456;
```

### 异或与开关

``` csharp
// 假设用二进制上的每一位表示独立开关
// a 表示变化前所有开关状态
byte a = 0b00000101;
// b 表示变化后所有开关状态
byte b = 0b00000111;
// 假设一次只能拨动一个开关，使得 a 变化到 b，则两者的异或结果表示拨动了哪一个开关
byte c = a ^ b;    // 0b00000010
```

### 隐式转换

``` csharp
public static implicit operator bool(Panel self) { return self != null; }
```

### 运算符重载