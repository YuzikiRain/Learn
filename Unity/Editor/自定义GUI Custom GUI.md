### 常用API

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
    
// GUILayoutOption，可用于GUILayout和EditorGUILayout的GUILayoutOption参数数组
GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight, GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.

// 常见控件的GUIStyle
EditorStyles.toolbarDropDown
    
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
```

### EditorWindow

``` csharp
[MenuItem("地图编辑器/编辑版块")]
    private static void Open()
    {
        var editorWindow = EditorWindow.GetWindow<CustomEditorWindow>();
        editorWindow.titleContent = new GUIContent("窗口标题");
    }

public class CustomEditorWindow : EditorWindow
{
	private OnGUI()
    {
        // 绘制一些GUI
    }
}
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


### 控件

### 常用控件

#### toolbar DropdownButton

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

#### SelectionGrid 可点击的Grid

``` csharp
int index = 0;
int columnCount = 3;
// grid的内容
ConfigGUIContent[] grids;
index = GUILayout.SelectionGrid(index, grids, columnCount);
```

可以配合ScrollView使用

#### PopupWindowContent 弹出框（其他区域点击后消失）

``` csharp
public class CustomPopupWindowContent : PopupWindowContent
{
    // 返回该content显示大小
    public override Vector2 GetWindowSize()
    {
        return new Vector2(200f, 200f);
    }
}
```

``` csharp
CustomPopupWindowContent content = new CustomPopupWindowContent();
PopupWindow.Show(rect, content);
```



#### SearchField

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
### 设置按钮

```GUILayout.Button(EditorGUIUtility.FindTexture("d__Popup@2x"), (GUIStyle)"SettingsIconButton");```

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

### CustomEditor

#### 让目标在Inspector上显示自定义内容

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

#### 迭代地显示自定义内容

参考UnityEditor.Editor.cs

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

 ``` csharp
 // UnityEditor.Editor.cs
 internal static bool DoDrawDefaultInspector(SerializedObject obj)
 {
 ...
     // 将 SerializedProperty iterator = obj.GetIterator();改为第一个要显示的SerializedProperty即可
     SerializedProperty iterator = serializedObject.FindProperty("FirstSerializedPropertyName");
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
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var barrageProperty = property.FindPropertyRelative("_barrages");
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

### 内置GUI资源

http://wiki.unity3d.com/index.php/Show_Built_In_Resources

### EditorStyles

![f:id:hacchi_man:20200331233616p:plain](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed@master/img/202201072202261.png)

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
}
```

参考：https://hacchi-man.hatenablog.com/entry/2020/04/06/220000

### 参考

-   https://mp.weixin.qq.com/s/8ANiIlsjOEZQEvQtr96Xug##

