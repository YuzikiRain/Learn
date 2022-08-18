``` c#
using UnityEditor;
using UnityEngine;

public class FindAssetByGUIDEditorWindow : EditorWindow
{
    string guid = "";
    string path = "";
    [MenuItem("SYZZ Tools/Asset/通过GUID找到资产 编辑器")]
    static void CreateWindow()
    {
        FindAssetByGUIDEditorWindow window = (FindAssetByGUIDEditorWindow)EditorWindow.GetWindow(typeof(FindAssetByGUIDEditorWindow));
        window.title = "通过GUID找到资产 编辑器";
    }

    void OnGUI()
    {
        GUILayout.Label("GUID");
        guid = GUILayout.TextField(guid);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("搜索", GUILayout.Width(120))) path = GetAssetPath(guid);
        if (GUILayout.Button("定位文件", GUILayout.Width(120)) && !string.IsNullOrEmpty(path)) EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(path));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label(path);
    }
    static string GetAssetPath(string guid)
    {
        string p = AssetDatabase.GUIDToAssetPath(guid);
        Debug.Log(p);
        if (p.Length == 0) p = "GUID对应的资产不存在";
        return p;
    }
}
```

