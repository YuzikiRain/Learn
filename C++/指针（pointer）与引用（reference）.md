## 定义和赋值

```c++
int i = 0;
int *pi = &i;
char c = 'a';
char *pc = &c;
int j = 1;
// 将pi重新指向j
pi = &j;
```

- `&`**取地址运算符**的操作数必须是左值，因为只有左值才表示一个内存单元，才会有地址，运算结果是指针类型（因为只有指针类型能存储地址）。
- 注意`pi`和`pc`虽然是不同类型的指针变量，但它们的内存单元都占4个字节，因为要保存32位的**虚拟地址**（不是物理地址，常见的物理地址范围为几个GB，实际访问物理地址是操作系统通过内存映射实现的），同理，在64位平台上指针变量都占8个字节。

```c++
// 定义多个指针变量，每个都要有*号
int *p, *q;
// *号和类型关键字之间可以省略空格，但更容易看错
int*a,*b;
// **注意！**这样定义了一个指针变量和int变量，而不是2个指针变量，所以建议不要省略空格，也不要遗漏*号
int*c,d;
```

## `*`解引用（Dereference）/间接引用（Indirection）操作符

```c++
int i = 0;
int *pi = &i;
*pi = *pi + 10;
```

将（指针`pi`所指向的）变量`i`的值增加10

## `->`和`.`操作符

``` c++
class Shader
{
public:
	unsigned int ID;
	void use(){}
}
```

``` c++
Shader* shader = new Shader();

(*shader).ID = 1;
(*shader).use();
// 以下写法效果一致
shader->ID = 2;
shader->use();

delete shader;
```

## 指针和引用的区别

指针可能指向某个实际对象，也可能指向0

引用必定要代表某个对象（因此使用时不需要判断是否为NULL或0）

``` C++
int intValue = 1024;				// int类型的对象
int *pointer = &intValue;			// pointer（指针），指向一个int对象
int &referenceValue = intValue;		// reference（引用），代表一个int对象

int anotherValue = 4096;
// 编译错误，referenceValue已经代表了intValue，无法转而代表anotherValue，C++不允许我们改变reference所代表的对象，它们必须从一而终
referenceValue = anotherValue;

// 将referenceValue所代表的对象（即intValue）的地址赋值给pointer，pointer指向intValue（而不是referenceValue）
// 注意，面对reference的所有操作都和面对“reference所代表的对象”所进行的操作一致
pointer = &referenceValue;
```

## 按值传递和按引用传递

当 我们 调用 一个 函数 时， 会在 内存 中 建立 起 一块 特殊 区域， 称为“ 程序 堆栈（ program stack）”。 这块 特殊 区域 提供 了 每个 函数 参数 的 储存 空间。 它 也 提供 了 函数 所 定义 的 每个 对象 的 内存 空间—— 我们将 这些 对象 称为 local object（ 局部 对象）。 一旦 函数 完成， 这块 内存 就会 被 释放 掉， 或者说 是 从 程序 堆栈 中 被 pop 出来。

``` c++
// 无法工作！！val1和val2都是在函数堆栈中，且通过按值传递被拷贝了一份，即形参和实参是不同地址的变量
void swap(int val1, int val2)
{
    int temp = val1;
    val1 = val2;
    val2 = temp;
}

// 正确，实参和形参都代表同一个地址的变量
void swap(int &val1, int &val2)
{
    int temp = val1;
    val1 = val2;
    val2 = temp;
}
```



