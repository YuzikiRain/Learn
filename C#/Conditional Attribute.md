添加条件的方式

- 编制指令：在每个代码文件的最上方添加 ```#define CONDITION1```
- 编译器选项中设置
    ![image-20220331135932414](https://fastly.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220331135932414.png)

``` csharp
#define CONDITION1
#define CONDITION2
using System;
using System.Diagnostics;

class Test
{
    static void Main()
    {
        Console.WriteLine("Calling Method1");
        Method1(3);
        Console.WriteLine("Calling Method2");
        Method2();

        Console.WriteLine("Using the Debug class");
        Debug.Listeners.Add(new ConsoleTraceListener());
        Debug.WriteLine("DEBUG is defined");
    }

    [Conditional("CONDITION1")]
    public static void Method1(int x)
    {
        Console.WriteLine("CONDITION1 is defined");
    }

    // CONDITION1 or CONDITION2
    [Conditional("CONDITION1"), Conditional("CONDITION2")]
    public static void Method2()
    {
        Console.WriteLine("CONDITION1 or CONDITION2 is defined");
    }
}

/*
When compiled as shown, the application (named ConsoleApp)
produces the following output.

Calling Method1
CONDITION1 is defined
Calling Method2
CONDITION1 or CONDITION2 is defined
Using the Debug class
DEBUG is defined
*/
```



参考

https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.conditionalattribute?view=net-5.0