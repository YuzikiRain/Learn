
完整的打包一般包含以下部分
- YourGameName_Data: 这个目录包含Plugin程序集，其他程序集（包括一般程序集Assembly-CSharp, 一般编辑器脚本程序集Assembly-CSharp-Editor, 插件编辑器脚本程序集Assembly-CSharp-Editor-firstpass，一般插件程序集Assembly-CSharp-firstpass，等等）Resources，StreamingAssets（StreamingAsset文件夹中的内容则会原封不动的打入包中，适合放一些）

- 对于一些仅在编辑器中使用，但是又不想放在名为Editor的目录下的脚本，可以将脚本中的函数或者整个脚本用预处理器指令包裹起来，这使得脚本在编辑器中有效但不会在打包时被编译
``` csharp
#if UNITY_EDITOR
// your script
#endif
```

-
