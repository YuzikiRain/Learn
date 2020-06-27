**一般的用法：重命名某public或[Serialized]的字段后，会导致引用丢失，在重命名之前加上该标签保存引用则可以避免。**


# 防止重命名变量后丢失引用

```
using UnityEngine;
using UnityEngine.Serialization;

public class MyClass : MonoBehaviour
{
	// 旧的变量名为myValue，重命名为myNewValue
    [FormerlySerializedAs("myValue")]
    public string myNewValue;
}
```


# 官方示例：将字段改为属性后，防止引用丢失

```
using UnityEngine;
using UnityEngine.Serialization;

public class MyClass : MonoBehaviour
{
	// 将字段m_MyValue改为属性myValue，并防止丢失其引用
    [FormerlySerializedAs("myValue")]
    string m_MyValue;
    public string myValue
    {
        get { return m_MyValue; }
        set { m_MyValue = value; }
    }
}
```

