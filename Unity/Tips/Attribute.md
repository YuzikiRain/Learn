## Inspector

``` csharp
// 纵向间隔
[Space(100)]
// 在变量上方的粗体Label
[Header("LabelName")] 
```

## 其他

``` csharp


// order为调用顺序
[PostProcessBuildAttribute(1)]
public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) 
{
    Debug.Log(pathToBuiltProject);
}
```



### 参考

-   https://docs.unity3d.com/ScriptReference/Callbacks.PostProcessBuildAttribute.html