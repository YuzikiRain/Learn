## 常用API

```csharp
// 找到已序列化字段
SerializedProperty property = serializedObject.FindProperty(fieldName);
// 从序列化字段中查找字段
property.FindPropertyRelative(fieldName)
// 如果在编辑器上修改了serializedObject的一些字段，必须调用该函数才能保存
serializedObject.ApplyModifiedProperties();

// 在GUILayout.BeginVertical之后使用，空行，高度为一行的高度
EditorGUILayout.Space(EditorGUIUtility.singleLineHeight)
// 在GUILayout.BeginVertical之后使用，使得之后的GUILayout绘制的GUI被排列到最后一行
GUILayout.Space(position.height - EditorGUIUtility.singleLineHeight * (1f + 1f + 0.5f));
// 设置label宽度，对之后的所有物体都生效，所以一般在对的下一行的物体应用后再设置回之前的宽度（设置为0f会用默认值）
EditorGUIUtility.labelWidth = 300f;
// 将包裹的绘制GUI变为只读的
EditorGUI.BeginDisabledGroup(isDisabled);
// 绘制一些GUI
EditorGUI.EndDisabledGroup();
// 返回默认类型的GUIStyle
GUIStyle guiStyle = EditorStyles.toolbarDropDown;
// 返回一个足够容纳 GUIContent 的指定 GUIStyle 类型的Rect
Rect rect = GUILayoutUtility.GetRect(new GUIContent("Content"), guiStyle);

// 绘制常见控件
GUILayout.Button
EditorGUILayout.Toggle
EditorGUILayout.ToggleLeft
EditorGUILayout.Popup
EditorGUILayout.TextField
EditorGUILayout.BeginHorizontal
EditorGUI.DropdownButton
// 根据类型进行默认绘制
EditorGUILayout.PropertyField(serializedProperty)
    
// 大按钮
GUILayout.Button("搜索label", GUILayout.MaxHeight(height), GUILayout.MaxWidth(width))
// 控制长度的Popup
GUILayout.BeginHorizontal();
// Popup总宽度100f，label宽度70f，剩余30f的宽度为弹出菜单
EditorGUIUtility.labelWidth = labelWidth;
EditorGUILayout.Popup("label总数：", labelLength, editorLabelsLengthArray, GUILayout.Width(popupWidth + labelWidth));
GUILayout.FlexibleSpace();
EditorGUIUtility.labelWidth = 0f;
GUILayout.EndHorizontal();

// 设置GUI颜色
var prevColor = GUI.color;
GUI.color = isValid ? Color.green : Color.red;
UnityEditor.EditorGUILayout.LabelField($"资源路径 {(isValid ? "合法" : "非法")}");
GUI.color = prevColor;
// GUILayoutOption，可用于GUILayout和EditorGUILayout的GUILayoutOption参数数组
GUILayout.Width(5f), GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.

// 常见控件的GUIStyle
EditorStyles.toolbarDropDown
    
// 是否有多个值（比如：LayerMask）
SerializedProperty.hasMultipleDifferentValues
// 多选时，如果Property有不同的值，是否显示mixValue符号
EditorGUI.showMixedValue
// 默认label的gui样式
GUIStyle style = GUI.skin.label;
// 是否使用Dark主题
UnityEditor.EditorGUIUtility.isProSkin
    
// 使得父Rect的宽度变化时，A GUI 和B GUI 大小仍保持不变，因为多出的宽度被FlexibleSpace使用了
GUILayout.BeginHorizontal();
// 绘制A GUI
...
GUILayout.FlexibleSpace();
// 绘制一些间隔
GUILayout.Space(2f);
// 绘制B GUI
...
GUILayout.EndHorizontal();
// 缩进
EditorGUI.indentLevel++;

// 检查编辑器上的属性是否被修改
EditorGUI.BeginChangeCheck();
some code may change editor
if (EditorGUI.EndChangeCheck())
{
    SetKeyword("_METALLIC_MAP", map.textureValue);
}

// 取得上一次使用的rect区域，仅能在OnGUI中使用
if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
```

## 基本概念

### SerializedObject

序列化物体，嵌套序列化字段

非Unity自带类型应该用`[System.Serializable]`

``` c#
[System.Serializable]
class A
{
    public int fieldA;
    public B fieldB;
}

[System.Serializable]
class B
{
    public string fieldC;
}
```

常用于编辑器显示

``` c#
//c# 示例 (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

// 修饰CustomEditor(typeof(目标类))
[CustomEditor(typeof(LookAtPoint))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    SerializedProperty lookAtPoint;
    
    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        // 找到lookAtPoint下的一个名为 fieldOflookAtPoint 的已序列化字段
        lookAtPoint.FindPropertyRelative("fieldOflookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lookAtPoint);
        // 如果在编辑器上修改了serializedObject的一些字段，必须调用该函数才能保存
        serializedObject.ApplyModifiedProperties();
    }
}
```

[Unity - Scripting API: SerializedObject (unity3d.com)](https://docs.unity3d.com/ScriptReference/SerializedObject.html)

#### 常用操作

``` c#
// 找到已序列化字段
SerializedProperty property = serializedObject.FindProperty(fieldName);
// 如果在编辑器上修改了serializedObject的一些字段，必须调用该函数才能保存
serializedObject.ApplyModifiedProperties();
```

### SerializedProperty

序列化字段，可再嵌套其他序列化字段

#### 常用操作

``` c#
// 从序列化字段中查找字段
property.FindPropertyRelative(fieldName)
```

### Layout

GUI和GUILayout可用于编辑器和Player，EditorGUI和EditorGUILayout仅能用于编辑器

Layout有自动布局效果，容易实现自适应的界面。

#### Area

**务必使用```GUILayout.BeginArea```和```GUILayout.EndArea```来嵌套布局语句，否则自动布局系统无法作用在正确区域上**

https://docs.unity3d.com/ScriptReference/GUILayout.BeginArea.html

#### ScrollView 滚动视图

``` csharp
scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width));
// 绘制一些内容
EditorGUILayout.EndScrollView();
```

## 控件

### toolbar DropdownButton

``` csharp
GUILayout.BeginHorizontal(EditorStyles.toolbar);

var guiMode = new GUIContent("Create");
Rect rMode = GUILayoutUtility.GetRect(guiMode, EditorStyles.toolbarDropDown);
if (EditorGUI.DropdownButton(rMode, guiMode, FocusType.Passive, EditorStyles.toolbarDropDown))
{
    // 创建Menu
    var menu = new GenericMenu();
    foreach (var templateObject in settings.GroupTemplateObjects)
    {
        // 为menu添加选项
        menu.AddItem(new GUIContent("Group/" + templateObject.name), false, m_EntryTree.CreateNewGroup, templateObject);
    }
    // 添加分隔符
    menu.AddSeparator(string.Empty);
    // 显示menu所有选项
    menu.DropDown(rMode);
}

GUILayout.EndHorizontal();
```

### SelectionGrid 可点击的Grid

``` csharp
int index = 0;
int columnCount = 3;
// grid的内容
ConfigGUIContent[] grids;
index = GUILayout.SelectionGrid(index, grids, columnCount);
```

可以配合ScrollView使用

### SearchField

##### 居中的搜索框

```csharp
private GUIStyle searchStyle;
private void InitStyle()
{
	searchStyle = GUI.skin.FindStyle("ToolbarSeachTextFieldPopup");
    if (searchStyle == null) searchStyle = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("ToolbarSeachTextFieldPopup");
    SearchField searchField = new SearchField();
}

private void OnGUI()
{
    InitStyle();
    // 用一定百分比的当前window的宽度作为搜索栏的最大宽度
    Rect searchRect = GUILayoutUtility.GetRect(0f, position.width * 0.6f, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight, searchStyle);
    assetsTreeView.searchString = searchField.OnGUI(searchRect, assetsTreeView.searchString);
}
```
### PopupWindowContent 弹出框（其他区域点击后消失）

``` csharp
using UnityEngine;
using UnityEditor;

public class EditorWindowWithPopup : EditorWindow
{
    // Add menu item
    [MenuItem("Example/Popup Example")]
    static void Init()
    {
        EditorWindow window = EditorWindow.CreateInstance<EditorWindowWithPopup>();
        window.Show();
    }

    Rect buttonRect;
    void OnGUI()
    {
        {
            GUILayout.Label("Editor window with Popup example", EditorStyles.boldLabel);
            if (GUILayout.Button("Popup Options", GUILayout.Width(200)))
            {
                // 第一个参数rect的左上角就是CustomPopupWindowContent的初始位置
                PopupWindow.Show(buttonRect, new CustomPopupWindowContent());
            }
            // 取得上一次使用的rect区域
            if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
        }
    }
}

public class CustomPopupWindowContent : PopupWindowContent
{
    // 返回该content显示大小
    public override Vector2 GetWindowSize()
    {
        return new Vector2(200f, 200f);
    }
    
    public override void OnGUI(Rect rect)
    {

    }
}
```

``` csharp
CustomPopupWindowContent content = new CustomPopupWindowContent();
PopupWindow.Show(rect, content);
```

参考：https://docs.unity3d.com/ScriptReference/PopupWindow.html

## 自定义Inspector显示

### CustomEditor

#### 让目标在Inspector上显示自定义内容

serializedObject是一个隐含的变量，表示目标类型的序列化物体，名称不能变

``` csharp
//c# 示例 (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

// 修饰CustomEditor(typeof(目标类))
[CustomEditor(typeof(LookAtPoint))]
[CanEditMultipleObjects]
public class LookAtPointEditor : Editor
{
    SerializedProperty lookAtPoint;
    
    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        // 找到lookAtPoint下的一个名为 fieldOflookAtPoint 的已序列化字段
        lookAtPoint.FindPropertyRelative("fieldOflookAtPoint");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(lookAtPoint);
        // 如果在编辑器上修改了serializedObject的一些字段，必须调用该函数才能保存
        serializedObject.ApplyModifiedProperties();
    }
}
```

[Unity - Manual: Custom Editors (unity3d.com)](https://docs.unity3d.com/Manual/editor-CustomEditors.html)

#### 迭代地显示自定义内容

参考UnityEditor.Editor.cs，这里是从当前Mono脚本去迭代地查找SerializedProperty即每个被序列化的字段，然后再对每个去迭代地显示

```csharp
// UnityEditor.Editor.cs
internal static bool DoDrawDefaultInspector(SerializedObject obj)
{
    EditorGUI.BeginChangeCheck();
    obj.UpdateIfRequiredOrScript();
    SerializedProperty iterator = obj.GetIterator();
    bool enterChildren = true;
    while (iterator.NextVisible(enterChildren))
    {
        using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
        {
            EditorGUILayout.PropertyField(iterator, true);
        }

        enterChildren = false;
    }

    obj.ApplyModifiedProperties();
    return EditorGUI.EndChangeCheck();
}
```

所以如果要自定义地迭代显示（改变显示顺序，或增加、减少一些显示），就需要自己把要显示的字段的SerializedProperty挑出来，只利用`while (iterator.NextVisible(enterChildren))`来显示自己想显示的字段即可

 ``` csharp
 // UnityEditor.Editor.cs
 internal static bool DoDrawDefaultInspector(SerializedObject obj)
 {
 ...
     // 将 SerializedProperty iterator = obj.GetIterator();改为要显示的SerializedProperty即可
     SerializedProperty iterator = serializedObject.FindProperty("YourSerializedPropertyName");
 ...
 }
 ```

### CustomPropertyDrawer

CustomPropertyDrawer 直接影响 SerializedProperty 在Inspector上的显示（如果有CustomEditor(目标类)，且CustomPropertyDrawer所作用的类是目标类的字段，会被CustomEditor的OnInspectorGUI覆盖） 

相比于 CustomEditor，如果要自定义某个类的某字段的某字段的显示，CustomEditor 需要使用 ```FindPropertyRelative``` 多层查找才能找到目标，而 CustomPropertyDrawer 则只需要重写```OnGUI```方法直接对 property 进行自定义显示

``` csharp
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BarrageBehaviour))]
public class BarragePropertyDrawer : PropertyDrawer
{
    private static string[] triggerTypeValues = new string[] { "空", "接近时", "交互时", };
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var barrageProperty = property.FindPropertyRelative("_barrages");
        // 注意：设置label宽度，否则之后的triggerTypeRect计算时不会考虑到宽度，可能width或height变为负数
		EditorGUIUtility.labelWidth = 40;
		var triggerTypeRect = new Rect(position) { width = 100f, };
		barrageProperty.intValue = EditorGUI.Popup(triggerTypeRect, "触发类型", barrageProperty.intValue, triggerTypeValues);

    }
}
```

### EditorWindow

``` csharp
using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow
{
    string myString = "Hello World";

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = EditorWindow.GetWindow<MyWindow>();
        window.Show();
    }

    [MenuItem("Window/Close My Window")]
    static void Close()
    {
        // Checks if any window of type MyWindow is open
        // 某类型的EditorWindow是否已经打开
        if (EditorWindow.HasOpenInstances<MyWindow>())
        {
            var window = EditorWindow.GetWindow<MyWindow>();
            window.Close();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
    }
}
```

### CustomShaderGUI

```csharp
public class CustomLitGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var property = FindProperty(propertyName, properties);
        // 如果选中了多个材质，材质上的对应属性可能有不同的值
        // editor.ShaderProperty会自动处理这些，但是如果用EditorGUI等绘制则需要手动处理
        EditorGUI.showMixedValue = property.hasMixedValue;
        // EditorGUILayout 或 EditorGUI 来绘制属性
        // 绘制完毕后记得重置
        EditorGUI.showMixedValue = false;
    }
```

## 杂项

### inspector上显示LayerMask

``` c#
// layerMaskProperty是LayerMask对应的SerializedProperty
// 获得修改后的值
LayerMask tempMask = UnityEditor.EditorGUILayout.MaskField("LayerMask", UnityEditorInternal.InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMaskProperty.intValue), UnityEditorInternal.InternalEditorUtility.layers);
// 应用到序列化字段上
layerMaskProperty.intValue = UnityEditorInternal.InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
```

### 自定义ProjectSettings或Preferences选项SettingsProvider

``` csharp
class CustomSettingProvider : SettingsProvider
{
    public BordlessFrameworkSettingProvider(string path, SettingsScope scopes = SettingsScope.Project) : base(path, scopes) { }

    [SettingsProvider]
    private static SettingsProvider ShowSettingsProvider()
    {
        return new CustomSettingProvider($"CustomSetting/Log Switch");
    }
}
```

参考：

https://docs.unity3d.com/2019.4/Documentation/ScriptReference/SettingsProvider.html

### 可排序列表 ReorderableList

虽然从2020版本开始已经默认支持了，但是如果要自定义每个元素的显示时（比如通过PropertyDrawer），仍需要自定义编写

[Unity: make your lists functional with ReorderableList (lent.in)](https://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/)

## 内置GUI资源

http://wiki.unity3d.com/index.php/Show_Built_In_Resources

## EditorStyles

![f:id:hacchi_man:20200331233616p:plain](https://fastly.jsdelivr.net/gh/YuzikiRain/ImageBed@master/img/202201072202261.png)

虽然下面没有描述， * EditorStyles.toolbarSearchField * EditorStyles.foldoutHeader * EditorStyles.foldoutHeaderIcon 从 2019.1 开始可用。

```cs
private void OnGUI()
{
    EditorGUILayout.LabelField("label", EditorStyles.label);
    EditorGUILayout.LabelField("miniLabel", EditorStyles.miniLabel);
    EditorGUILayout.LabelField("largeLabel", EditorStyles.largeLabel);
    EditorGUILayout.LabelField("boldLabel", EditorStyles.boldLabel);
    EditorGUILayout.LabelField("miniBoldLabel", EditorStyles.miniBoldLabel);
    EditorGUILayout.LabelField("centeredGreyMiniLabel", EditorStyles.centeredGreyMiniLabel);
    EditorGUILayout.LabelField("wordWrappedMiniLabel", EditorStyles.wordWrappedMiniLabel);
    EditorGUILayout.LabelField("wordWrappedLabel", EditorStyles.wordWrappedLabel);
    EditorGUILayout.LabelField("whiteLabel", EditorStyles.whiteLabel);
    EditorGUILayout.LabelField("whiteMiniLabel", EditorStyles.whiteMiniLabel);
    EditorGUILayout.LabelField("whiteLargeLabel", EditorStyles.whiteLargeLabel);
    EditorGUILayout.LabelField("whiteBoldLabel", EditorStyles.whiteBoldLabel);
    EditorGUILayout.LabelField("radioButton", EditorStyles.radioButton);
    EditorGUILayout.LabelField("miniButton", EditorStyles.miniButton);
    using (new EditorGUILayout.HorizontalScope())
    {
        EditorGUILayout.LabelField("miniButtonLeft", EditorStyles.miniButtonLeft);
        EditorGUILayout.LabelField("miniButtonMid", EditorStyles.miniButtonMid);
        EditorGUILayout.LabelField("miniButtonRight", EditorStyles.miniButtonRight);
    }
    EditorGUILayout.LabelField("miniPullDown", EditorStyles.miniPullDown);
    EditorGUILayout.LabelField("textField", EditorStyles.textField);
    EditorGUILayout.LabelField("textArea", EditorStyles.textArea);
    EditorGUILayout.LabelField("miniTextField", EditorStyles.miniTextField);
    EditorGUILayout.LabelField("numberField", EditorStyles.numberField);
    EditorGUILayout.LabelField("popup", EditorStyles.popup);
    EditorGUILayout.LabelField("structHeadingLabel", EditorStyles.structHeadingLabel);
    EditorGUILayout.LabelField("objectField", EditorStyles.objectField);
    EditorGUILayout.LabelField("objectFieldThumb", EditorStyles.objectFieldThumb);
    EditorGUILayout.LabelField("objectFieldMiniThumb", EditorStyles.objectFieldMiniThumb);
    EditorGUILayout.LabelField("colorField", EditorStyles.colorField);
    EditorGUILayout.LabelField("layerMaskField", EditorStyles.layerMaskField);
    EditorGUILayout.LabelField("toggle", EditorStyles.toggle);
    EditorGUILayout.LabelField("foldout", EditorStyles.foldout);
    EditorGUILayout.LabelField("foldoutPreDrop", EditorStyles.foldoutPreDrop);
    EditorGUILayout.LabelField("toggleGroup", EditorStyles.toggleGroup);
    EditorGUILayout.LabelField("toolbar", EditorStyles.toolbar);
    GUILayout.Space(5);
    using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
    {
        EditorGUILayout.LabelField("toolbarButton", EditorStyles.toolbarButton);
        EditorGUILayout.LabelField("toolbarPopup", EditorStyles.toolbarPopup);
        EditorGUILayout.LabelField("toolbarDropDown", EditorStyles.toolbarDropDown);
    }
    GUILayout.Space(5);
    using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
    {
        EditorGUILayout.LabelField("toolbarTextField", EditorStyles.toolbarTextField);
    }
    EditorGUILayout.LabelField("inspectorDefaultMargins", EditorStyles.inspectorDefaultMargins);
    EditorGUILayout.LabelField("inspectorFullWidthMargins", EditorStyles.inspectorFullWidthMargins);
    EditorGUILayout.LabelField("helpBox", EditorStyles.helpBox);
    // 设置按钮
	GUILayout.Button(EditorGUIUtility.FindTexture("d__Popup@2x"), (GUIStyle)"SettingsIconButton");
}
```

参考：https://hacchi-man.hatenablog.com/entry/2020/04/06/220000

### 参考

-   https://mp.weixin.qq.com/s/8ANiIlsjOEZQEvQtr96Xug##

