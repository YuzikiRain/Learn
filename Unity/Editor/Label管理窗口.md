``` csharp
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

class LabelWindow : EditorWindow
{
    [MenuItem("LabelTools/Label管理窗口" + " #&g")]
    static void Open()
    {
        Init();
        LabelWindow window = EditorWindow.GetWindow<LabelWindow>("Label管理窗口");
        window.Show();
    }

    [DidReloadScripts]
    private static void Init()
    {
        editorLabelsLengthArray = new string[labelCapacity + 1];
        for (int i = 0; i < editorLabelsLengthArray.Length; i++)
        {
            editorLabelsLengthArray[i] = i.ToString();
        }
    }

    private const string editorLabelsLength = "EditorSearchLabelsLength";
    private const string editorLabels = "EditorSearchLabels";
    private static string[] editorLabelsLengthArray;
    private static string[] labels;
    private static int labelLength = -1;

    private const int labelCapacity = 10;
    private const float space = 5f;
    private const float height = 60f;
    private const float labelWidth = 70f;
    private const float popupWidth = 40f;
    void OnGUI()
    {
        if (labels == null)
        {
            var s = EditorPrefs.GetString(editorLabels);
            labels = s.Split(',');
        }
        if (labelLength == -1) labelLength = EditorPrefs.GetInt(editorLabelsLength, 0);

        // labels
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        for (int i = 0; i < labelLength; i++)
        {
            labels[i] = GUILayout.TextField(labels[i]);
        }
        GUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            var saveInputs = "";
            foreach (var input in labels) saveInputs += $"{input},";
            EditorPrefs.SetString(editorLabels, saveInputs);
        }

        // labels长度
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        // Popup总宽度100f，label宽度70f，剩余30f的宽度为弹出菜单
        EditorGUIUtility.labelWidth = labelWidth;
        labelLength = EditorGUILayout.Popup("label总数：", labelLength, editorLabelsLengthArray, GUILayout.Width(popupWidth + labelWidth));
        GUILayout.FlexibleSpace();
        EditorGUIUtility.labelWidth = 0f;
        GUILayout.EndHorizontal();
        //labelLength = int.Parse(GUILayout.TextField("label总数", labelLength.ToString()));
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetInt(editorLabelsLength, labelLength);
            var old = labels;
            labels = new string[labelLength];
            for (int i = 0; i < labelLength; i++)
            {
                labels[i] = (i < old.Length && old[i] != null) ? old[i] : string.Empty;
            }
        }
        GUILayout.Space(space);

        if (GUILayout.Button("搜索label", GUILayout.MaxHeight(height))) SearchByLabels(labels);
        GUILayout.Space(space);
        if (GUILayout.Button("添加label")) AddLabels(labels);
        GUILayout.Space(space);
        if (GUILayout.Button("移除label")) RemoveLabels(labels);
        GUILayout.Space(space);
        if (GUILayout.Button("清空label")) ClearLabels();
    }

    [MenuItem("LabelTools/Label搜索" + " #&f")]
    private static void SearchByLabelsShortcut()
    {
        SearchByLabels(labels);
    }

    private static void SearchByLabels(string[] labels)
    {
        Type projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        EditorWindow window = EditorWindow.GetWindow(projectBrowserType);

        // UnityEditor.ProjectBrowser.SetSearch(string searchString)
        MethodInfo setSearchMethodInfo = projectBrowserType.GetMethod("SetSearch", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);

        string s = "";
        foreach (var label in labels)
        {
            if (!string.IsNullOrEmpty(label)) s += $"l:{label} ";
        }
        setSearchMethodInfo.Invoke(window, new object[] { s });
    }

    private static void AddLabels(string[] labels)
    {
        foreach (var obj in Selection.objects)
        {
            List<string> s = new List<string>(AssetDatabase.GetLabels(obj));
            foreach (var label in labels)
            {
                if (!string.IsNullOrEmpty(label)) s.Add(label);
            }
            AssetDatabase.SetLabels(obj, s.ToArray());
        }
    }

    private static void RemoveLabels(string[] labels)
    {
        foreach (var obj in Selection.objects)
        {
            List<string> s = new List<string>(AssetDatabase.GetLabels(obj));
            foreach (var label in labels)
            {
                if (s.Contains(label)) s.Remove(label);
            }
            AssetDatabase.SetLabels(obj, s.ToArray());
        }
    }

    private static void ClearLabels()
    {
        foreach (var obj in Selection.objects)
        {
            AssetDatabase.ClearLabels(obj);
        }
    }
}
```

