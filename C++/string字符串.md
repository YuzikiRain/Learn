``` c++
#include <iostream>
#include <cstring>
#include <typeinfo>
using namespace std;

int main() {
    // defaultName是字符数组，指向首个字符的地址
    char defaultName[] = "abcdefghijk";
    // strlen从头开始计数字符个数，直到遇到'\0'结束且不计入总数
	cout << "defaultName: type = " << typeid(defaultName).name() 
	<< ", value = " << defaultName 
	// defaultName实际上数组长度为4，最后一个字符是'\0'
	<< ", character count = " << strlen(defaultName)
	// *defaultName表示数组首个字符，sizeof返回首个字符所占内存空间，char类型，因此为1byte
	<< ", element size = " << sizeof(*defaultName)
	// 为字符数组defaultName分配了12个char的内存空间，每个char为1byte
	// 对于数组类型，sizeof返回(元素大小*元素总数)，因此sizeof返回12
	<< ", sizeof = " << sizeof(defaultName)
	<< "\n";
	// anotherName是字符指针，指向字符数组，也是指向首个字符的地址
	char* anotherName = defaultName;
	cout << "anotherName: type = " << typeid(anotherName).name() 
	<< ", value = " << anotherName
	<< ", character count = " << strlen(anotherName)
	// *anotherName表示指针指向的首个字符，sizeof返回首个字符所占内存空间，char类型，因此为1byte
	<< ", element size = " << sizeof(*anotherName)
	// anotherName是字符指针类型
	// sizeof(一维数组指针)返回的是数组单个元素指针的大小，即系统指针的长度，32位系统为4，64位系统位8
	<< ", sizeof = " << sizeof(anotherName)
	<< "\n";
	
	// 这里分配的内存空间为sizeof(defaultName)，或strlen(defaultName)+1
	char* copyedName = new char[strlen(defaultName)];
	// 否则这里copyedName的内存就会比defaultName少1byte，无法拷贝结束字符'\0'
	strcpy(copyedName, defaultName);
	return 0;
}
```

`sizeof` 运算符是一个一元运算符，用于检索给定表达式或数据类型的存储大小。此运算符以byte字节为单位评估对象大小，并且 `sizeof(char)` 保证为 `1`。

参考：

[C++ 字符串 | 菜鸟教程 (runoob.com)](https://www.runoob.com/cplusplus/cpp-strings.html)
