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
