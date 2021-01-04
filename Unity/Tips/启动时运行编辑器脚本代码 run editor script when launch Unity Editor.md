``` csharp
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Startup {
    static Startup()
    {
        Debug.Log("Up and running");
    }
}
```

参考

-   https://docs.unity3d.com/ScriptReference/InitializeOnLoadAttribute.html
-   https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html