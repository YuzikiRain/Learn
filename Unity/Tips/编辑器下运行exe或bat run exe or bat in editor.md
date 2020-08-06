### bat

- 打开bat文件，在第一行插入```cd /D %~dp0```
- 
    ```
    Application.OpenURL("xxx.bat");
    UnityEditor.EditorUtility.OpenWithDefaultApp("xxx.bat");
    ```

### exe
``` csharp
string args = $"your args";
// must be absolute path
var startInfo = new System.Diagnostics.ProcessStartInfo($"xxx.exe", args);
myprocess.StartInfo = startInfo;
myprocess.StartInfo.UseShellExecute = false;
myprocess.Start();
```

参考
[Chinarcsdn](https://blog.csdn.net/ChinarCSDN/article/details/81229490)