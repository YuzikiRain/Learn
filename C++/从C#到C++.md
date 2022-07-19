## C#委托->C++函数指针

``` c++
bool lengthCompare(const string &, const string &);
bool (*pf)(const string &, const string &);
```

pf是一个指向函数的指针，该函数的返回值是bool

[从 C# 移动到 C++/WinRT - UWP applications | Microsoft Docs](https://docs.microsoft.com/zh-cn/windows/uwp/cpp-and-winrt-apis/move-to-winrt-from-csharp)