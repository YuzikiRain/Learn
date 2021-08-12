``` csharp
using System.Reflection;
using UnityEditor;
using System;
using UnityEditor.Callbacks;
using System.Diagnostics;
using UnityEngine;
using System.IO;

public class LogRedirect
{
    private static readonly string OpenApplicationPath = "D:/软件/IntelliJ IDEA Community Edition 2021.2/bin/idea64.exe";
    private static readonly string[] SearchFolders = { "Assets/Lua" };
    private const string ErrorPrefix = "LuaException: ";
    private static object logListView;
    private static EditorWindow consoleWindow;
    private static FieldInfo logListViewCurrentRow;
    private static Type logEntryType;
    private static MethodInfo LogEntriesGetEntry;
    private static object logEntry;
    private static MethodInfo OpenFileOnSpecificLineAndColumn;

    private static bool GetConsoleWindowListView()
    {
        if (logListView == null)
        {
            Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            consoleWindow = fieldInfo.GetValue(null) as EditorWindow;

            if (consoleWindow == null)
            {
                logListView = null;
                return false;
            }

            FieldInfo listViewFieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
            logListView = listViewFieldInfo.GetValue(consoleWindow);
            logListViewCurrentRow = listViewFieldInfo.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);

            Type logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            OpenFileOnSpecificLineAndColumn = logEntriesType.GetMethod("OpenFileOnSpecificLineAndColumn", BindingFlags.Static | BindingFlags.Public);
            logEntryType = unityEditorAssembly.GetType("UnityEditor.LogEntry");
            logEntry = Activator.CreateInstance(logEntryType);
        }

        return true;
    }

    [OnOpenAssetAttribute(0)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        //只对控制台的开启进行重定向
        if (!EditorWindow.focusedWindow.titleContent.text.Equals("Console")) return false;

        if (!GetConsoleWindowListView()) return false;

        int row = (int)logListViewCurrentRow.GetValue(logListView);
        LogEntriesGetEntry.Invoke(null, new object[] { row, logEntry });
        string message = logEntryType.GetField("message").GetValue(logEntry).ToString();
        var match = System.Text.RegularExpressions.Regex.Match(message, @".+\.lua:[0-9]+");
        string fileName = null;
        if (match.Success)
        {
            int index0 = match.Value.LastIndexOf('[');
            int index1 = match.Value.LastIndexOf(":");
            int index2 = match.Value.LastIndexOf(".lua");
            fileName = match.Value.Substring(index0 + 1, index2 - index0 - 1);
            line = int.Parse(match.Value.Substring(index1 + 1));
        }
        else if (message.Contains(ErrorPrefix))
        {
            match = System.Text.RegularExpressions.Regex.Match(message, $@"{ErrorPrefix}.*:[0-9]+");
            int index0 = match.Value.LastIndexOf(ErrorPrefix);
            int index1 = match.Value.LastIndexOf(':');
            line = int.Parse(match.Value.Substring(index1 + 1));
            if (match.Success) fileName = match.Value.Substring(index0 + ErrorPrefix.Length, index1 - index0 - ErrorPrefix.Length);
        }

        if (fileName == null) return false;

        string[] searchPaths = AssetDatabase.FindAssets(fileName, SearchFolders);
        for (int i = 0; i < searchPaths.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(searchPaths[i]);
            if (path == null) continue;

            path = $"{Path.Combine(Directory.GetParent(Application.dataPath).FullName, path)}";
            processCommand(OpenApplicationPath, $"{path}:{line}");
            //OpenFileOnSpecificLineAndColumn.Invoke(null, new object[] { path, line, 0 });
            return true;
        }

        return false;
    }

    private static void processCommand(string command, string argument)
    {
        ProcessStartInfo start = new ProcessStartInfo(command);
        start.Arguments = argument;
        start.CreateNoWindow = false;
        start.ErrorDialog = true;
        start.UseShellExecute = true;
        if (start.UseShellExecute)
        {
            start.RedirectStandardOutput = false;
            start.RedirectStandardError = false;
            start.RedirectStandardInput = false;
        }
        else
        {
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.RedirectStandardInput = true;
            start.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            start.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        }
        Process p = Process.Start(start);
        //if (!start.UseShellExecute)
        //{

        //}
        //p.WaitForExit();
        //p.Close();
    }
}
```

### 参考

https://blog.csdn.net/suifcd/article/details/80085184