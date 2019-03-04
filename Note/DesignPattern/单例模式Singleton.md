- 创建私有变量 _instance,内部进行实例化（用以记录 Singleton 的唯一实例）。
- 把类的构造方法私有化，不让外部调用构造方法实例化。
- 定义公有方法或属性提供该类的全局唯一访问点。
```
using System;

 public class Singleton {
    //1. 创建私有变量 _instance,内部进行实例化（用以记录 Singleton 的唯一实例）
    private static Singleton _instance  = new  Singleton();

    //2. 把类的构造方法私有化，不让外部调用构造方法实例化
    private Singleton() {
    }
    //3. 定义公有方法或属性提供该类的全局唯一访问点
    public static  Singleton GetInstance() {
        return _instance;
    }
    public static Singleton Instance{
        get{
            return _instance;
        }
    }
    //这里的 mField1 是 _instance.Field1()
     string mField1 = "mField1";
    //这里的 GetField1 是 _instance.GetField1()
    public string GetField1()
    {
        Console.WriteLine("_instance call GetField1, return {0}", mField1);
        return mField1;
    }

}

public class Test
{
    public static void Main()
    {
        Singleton.Instance.GetField1();
    }
}  
```