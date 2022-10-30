

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

### 指向常量的指针（pointer to const）

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

从右往左阅读，const表示cp是不可变的常量，`*`表示cp是一个指针类型，int表示指针指向int