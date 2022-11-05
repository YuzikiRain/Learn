

## const对象

``` c++
int i = 42;
// 正确：i的值被拷贝给了ci
const int ci = i;
// 正确：ci的值被拷贝给了j
int j = ci;
// 错误，不能修改const对象
ci = 41
```

## const引用

对常量的引用，不能用作修改其绑定的对象（因为是一个const修饰的常量）

常量引用（仅）对（引用本身）可参与的操作做出了限定

常量引用可以指向一个非常量对象（但是无法修改该引用）

``` c++
int i = 32;
const int ci = 1024;
// 正确：引用和对应的对象都是常量
const int &ri = ci;
// 正确：常量引用可以指向一个非常量对象（但是无法修改ri2来修改i）
const int &ri2 = i;
// 错误：不能修改其引用的常量
ri = 42;
// 错误：不能让一个非常量引用指向一个常量对象
int &r = ci;
const double d = 22;
const double d2 = 23;
// 错误：ci2应当指向int常量（const int），而不能指向double常量
const int &ci2 = d;
// 错误：不能指向一个double变量
const int &ci3 = d2;
```

## const和指针

### 指向常量的指针（pointe to const）

不能通过该指针改变其所指向对象的值

``` c++
const double pi = 3.14;
// 正确
const double *cp = &pi;
// 错误：p是一个指向double的普通指针，不能指向const double
double *p = &pi;
// 错误：不能通过指针改变常量的值
*cp = 3.15
```

### 常量指针（const pointer）

指针是对象（而引用不是），因此就像其他类型的对象一样，可以将指针本身定为常量

常量指针必须初始化，一旦初始化后，其值也就是指向的地址不能在改变

其指向的变量可以被改变

``` c++
int a = 0;
// 正确
int *const cp = &a;
```

从右往左阅读，const表示cp是不可变的常量，`*`表示cp是一个指针类型，int表示指针指向int类型的对象

## 顶层const

指针本身是一个对象，它又可以指向另外一个对象。因此，指针本身是不是常量（常量指针），以及指针所指的是不是一个常量（指向常量的指针），是两个相互独立的问题。

对于指针，用**顶层const（top-level const）**表示指针本身是个常量，用**底层const（low-level const）**表示指针指向的对象是个常量。

更一般的，顶层const可以表示任意的对象是常量，这一点适用于任何数据类型，比如算术类型、类、指针等。而底层const则与指针和引用等复合类型的基本类型部分有关。（即指针既可以是顶层const，也可以是底层const）

## constexpr和常量表达式

**常量表达式（const experssion）**是指**值不会改变**并且在**编译过程中就能得到计算结果**的表达式。

- 字面值属于常量表达式
- 用常量表达式初始化的const对象也是常量表达式

一个对象（或表达式）是不是常量表达式是由它的数据类型和初始值共同决定的，例如：

``` c++
const int max_files = 20;				// max_files是常量表达式
const int limit = max_files + 1;		// limit是常量表达式
int staff_size = 30;					// staff_size不是常量表达式
const int sz = get_size();				// sz不是常量表达式
```

staff_size的初始值是个字面值常量，但是由于它的数据类型只是一个`int`而不是`const int`，所以不是常量表达式

sz是一个常量，但是具体值直到运行时才能获得，所以不是常量表达式

### constexpr变量

在一个复杂系统中，很难分辨一个初始值到底是不是常量表达式。即使定义了一个const变量并将它的初始值设为我们认为的某个常量表达式，但是实际使用时，却常常发现初始值并非真正的常量表达式。

C++11新标准规定，允许将变量声明为constexpr类型以便由编译器来验证变量的值是否是一个常量表达式。声明为constexpr的变量一定是一个常量，而且必须用常量表达式初始化：

``` c++
constexpr int mf = 20;
constexpr int limit = mf + 1;
constexpr int sz = size();			// 仅当size是一个constexpr函数时才是一条正确的声明语句
```

#### 字面值类型（literal type）

算术类型、引用、指针都属于字面值类型。

指针和引用都能定义成constexpr，但初始值却受到严格限制。一个constexpr指针的初始值必须是nullptr或者0，或存储于某个固定地址中的对象。

函数体内定义的变量一般来说并非存放在固定地址中，因此constexpr指针不能指向这样的变量。

相反，定义于函数体外的对象的地址固定不变，可以用于初始化constexpr指针。
