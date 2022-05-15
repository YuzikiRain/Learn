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
	// *defaultName表示首个字符，sizeof返回首个字符所占内存空间，char类型，因此为1byte
	<< ", element size = " << sizeof(*defaultName)
	// 为字符数组defaultName分配了12个byte大小的内存空间，因此sizeof返回12
	<< ", sizeof = " << sizeof(defaultName)
	<< "\n";
	// anotherName是字符指针，指向字符数组，也是指向首个字符的地址
	char* anotherName = defaultName;
	cout << "anotherName: type = " << typeid(anotherName).name() 
	<< ", value = " << anotherName
	<< ", character count = " << strlen(anotherName)
	// *anotherName表示首个字符，sizeof返回首个字符所占内存空间，char类型，因此为1byte
	<< ", element size = " << sizeof(*anotherName)
	// anotherName是字符指针类型
	// sizeof(一维数组指针)返回的是数组单个元素指针的大小，即系统指针的长度，32位系统为4，64位系统位8
	<< ", sizeof = " << sizeof(anotherName)
	<< "\n";
	return 0;
}
```

`sizeof` 运算符是一个一元运算符，用于检索给定表达式或数据类型的存储大小。此运算符以字节为单位评估对象大小，并且 `sizeof(char)` 保证为 `1`。
