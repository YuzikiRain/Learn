
``` csharp
//c# 示例 (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LookAtPoint))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor 
{
    SerializedProperty lookAtPoint;
    
    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lookAtPoint);
        serializedObject.ApplyModifiedProperties();
    }
}
```

-   GUILayout.Button
-   EditorGUILayout.Popup
-   EditorGUILayout.TextField

### Rect

两种表示方法

-   x y表示（屏幕坐标下的）位置，width height表示宽高
    ![image-20200910160644934](assets/image-20200910160644934.png)
-   xMin yMin xMax yMax表示（屏幕坐标下的）边界
    ![image-20200910160741851](assets/image-20200910160741851.png)