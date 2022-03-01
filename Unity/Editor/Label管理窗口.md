``` csharp
using BordlessFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace BordlessFramework
{
    class LabelEditorWindow : EditorWindow
    {
        [MenuItem("LabelTools/Label管理窗口" + " #&g")]
        static void Open()
        {
            Init();
            LabelEditorWindow window = EditorWindow.GetWindow<LabelEditorWindow>("Label管理窗口");
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

        private const string editorConfig = "EditorLabelConfig";
        private const string editorLabelsLength = "EditorSearchLabelsLength";
        private const string editorLabels = "EditorSearchLabels";
        private static LabelEditorWindowConfig labelConfig;
        private static string[] editorLabelsLengthArray;
        private static string[] labels;
        private static int labelLength = -1;

        private const int labelCapacity = 10;
        private const float space = 5f;
        private const float height = 60f;
        private const float labelWidth = 70f;
        private const float popupWidth = 40f;
        private const float toggleWidth = 15f;

        [Serializable]
        class LabelEditorWindowConfig
        {
            public int length;
            public LabelConfig[] labels;
        }

        [Serializable]
        class LabelConfig
        {
            public string value;
            public bool isOn;
        }

        void OnGUI()
        {
            if (labelConfig == null)
            {
                var temp = JsonUtility.FromJson<LabelEditorWindowConfig>(EditorPrefs.GetString(editorConfig));
                labelConfig = temp != null ? temp : new LabelEditorWindowConfig() { length = 0, labels = new LabelConfig[0] };
            }

            if (labels == null)
            {
                var s = EditorPrefs.GetString(editorLabels);
                labels = s.Split(',');
            }
            if (labelLength == -1) labelLength = EditorPrefs.GetInt(editorLabelsLength, 0);

            EditorGUI.BeginChangeCheck();

            // label length
            GUILayout.BeginHorizontal();
            // Popup总宽度100f，label宽度70f，剩余30f的宽度为弹出菜单
            EditorGUIUtility.labelWidth = labelWidth;
            labelConfig.length = EditorGUILayout.Popup("label总数：", labelConfig.length, editorLabelsLengthArray, GUILayout.Width(popupWidth + labelWidth));
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 0f;
            GUILayout.EndHorizontal();

            // labels
            for (int i = 0; i < labelConfig.labels.Length; i++)
            {
                GUILayout.BeginHorizontal();
                var label = labelConfig.labels[i];
                label.isOn = GUILayout.Toggle(label.isOn, string.Empty, GUILayout.Width(toggleWidth));
                label.value = GUILayout.TextField(label.value);
                GUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (labelConfig.labels.Length < labelConfig.length)
                {
                    var old = labelConfig.labels;
                    labelConfig.labels = new LabelConfig[labelConfig.length];
                    for (int i = 0; i < labelConfig.labels.Length; i++)
                    {
                        labelConfig.labels[i] = i < old.Length ?
                            new LabelConfig() { isOn = old[i].isOn, value = old[i].value }
                            : new LabelConfig() { isOn = false, value = string.Empty };
                    }
                }
                else
                {
                    var old = labelConfig.labels;
                    labelConfig.labels = new LabelConfig[labelConfig.length];
                    for (int i = 0; i < labelConfig.labels.Length; i++)
                    {
                        labelConfig.labels[i] = new LabelConfig() { isOn = old[i].isOn, value = old[i].value };
                    }
                }
                EditorPrefs.SetString(editorConfig, JsonUtility.ToJson(labelConfig));
            }

            GUILayout.Space(space);
            if (GUILayout.Button("搜索label", GUILayout.MaxHeight(height))) SearchByLabels(labelConfig.labels);
            GUILayout.Space(space);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("添加label")) AddLabels(Selection.objects, labelConfig.labels);
            if (GUILayout.Button("添加label（包括子文件）")) AddLabels(Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets), labelConfig.labels);
            GUILayout.EndHorizontal();
            GUILayout.Space(space);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("移除label")) RemoveLabels(Selection.objects, labelConfig.labels);
            if (GUILayout.Button("移除label（包括子文件）")) RemoveLabels(Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets), labelConfig.labels);
            GUILayout.EndHorizontal();
            GUILayout.Space(space);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("清空label")) ClearLabels(Selection.objects, labelConfig.labels);
            if (GUILayout.Button("清空label（包括子文件）")) ClearLabels(Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets), labelConfig.labels);
            GUILayout.EndHorizontal();
        }

        [MenuItem("LabelTools/Label搜索" + " #&f")]
        private static void SearchByLabelsShortcut()
        {
            SearchByLabels(labelConfig.labels);
        }

        // UnityEditor.ProjectBrowser.SetSearch(string searchString)
        private static MethodInfo setSearchMethodInfo = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor").GetMethod("SetSearch", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);
        private static void SearchByLabels(LabelConfig[] labels)
        {
            Type projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
            EditorWindow window = EditorWindow.GetWindow(projectBrowserType);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var label in labels)
            {
                if (label.isOn && !string.IsNullOrEmpty(label.value)) stringBuilder.Append($"l:{label.value} ");
            }
            setSearchMethodInfo.Invoke(window, new object[] { stringBuilder.ToString() });
        }

        // 添加标签
        private static void AddLabels(UnityEngine.Object[] selectedObjs, LabelConfig[] labels)
        {
            foreach (var obj in selectedObjs)
            {
                HashSet<string> currentlabels = new HashSet<string>(AssetDatabase.GetLabels(obj));
                foreach (var label in labels)
                {
                    if (label.isOn && !string.IsNullOrEmpty(label.value)) currentlabels.Add(label.value);
                }
                AssetDatabase.SetLabels(obj, currentlabels.ToArray());
            }
        }

        private static void RemoveLabels(UnityEngine.Object[] selectedObjs, LabelConfig[] labels)
        {
            foreach (var obj in selectedObjs)
            {
                HashSet<string> currentlabels = new HashSet<string>(AssetDatabase.GetLabels(obj));
                foreach (var label in labels)
                {
                    if (label.isOn && !string.IsNullOrEmpty(label.value)) currentlabels.Remove(label.value);
                }
                AssetDatabase.SetLabels(obj, currentlabels.ToArray());
            }
        }

        private static void ClearLabels(UnityEngine.Object[] selectedObjs, LabelConfig[] labels)
        {
            foreach (var obj in selectedObjs)
            {
                AssetDatabase.ClearLabels(obj);
            }
        }

    }
}
```

