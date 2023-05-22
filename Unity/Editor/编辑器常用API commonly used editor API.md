## Prefab

``` csharp
// PrefabInstanceStatus：预制体状态，NotAPrefab不是预制体，Connected预制体，MissingAsset引用的预制体资源丢失，Disconnected旧版本功能已弃用
PrefabUtility.GetPrefabInstanceStatus(UnityEngine.Object componentOrGameObject)
// 预制体，且不在场景中（在Asset中，是资源） is a prefab in asset
var isPartOfPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(gameObject);
// 预制体，且在场景中   is a prefab in hierarchy(scene) as instance
var isPartOfPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(gameObject);
// 从prefabInstance的子物体中取得root
var prefabInstanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(prefabInstance);
// 从Object获得prefab，应配合 PrefabUtility.GetOutermostPrefabInstanceRoot 使用，因为如果参数不是 prefabInstanceRoot 会返回 null
var prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(prefabInstanceRoot);
// 如果prefabInstanceRoot是嵌套预制体中的子prefab，使用GetCorrespondingObjectFromOriginalSource会返回子prefab（transform属性的parent为null），AssetDatabase.GetAssetPath会返回子prefab的path
// 而GetCorrespondingObjectFromSource则返回父prefab下的子prefab（可通过transform属性的parent一直往上找到父prefab），且AssetDatabase.GetAssetPath(childPrefabInstance)只会返回父prefab的path
var prefabAsset = PrefabUtility.GetCorrespondingObjectFromOriginalSource(prefabInstanceRoot);
// 从prefab资产中取得对应路径
var prefabAssetPath = AssetDatabase.GetAssetPath(prefabAsset);
// 如果参数 prefabInstanceRoot 确实是prefab的根物体，返回 null 表示在预制体模式下，否则在一般场景下
bool isInPrefabStage = prefabInstance != null;

// 用预制体资源创建预制体实例，并保持与预制体资源连接
PrefabUtility.InstantiatePrefab(prefab);
// 获得预制体场景下的根物体，即预制体根物体本身
PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
root = prefabStage.prefabContentsRoot;
// 使用实例创建prefab并连接
PrefabUtility.SaveAsPrefabAssetAndConnect
PrefabUtility.SaveAsPrefabAsset
```

## Stage

```csharp
// 在预制体模式下，获得预制体场景
PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
// （从PrefabStage或其他Stage）回到主场景
StageUtility.GoToMainStage();
// 返回上一个stage（比如prefab的stage）
StageUtility.GoBackToPreviousStage();
// 获得主stage
StageUtility.GetMainStage();
```

## EditorScene

```csharp
// 编辑器模式下获得当前激活场景
EditorSceneManager.GetActiveScene();
// 编辑器模式下关闭场景
EditorSceneManager.CloseScene
// 编辑器模式下关闭PreviewScene场景（不可用于PrefabStage场景）
EditorSceneManager.ClosePreviewScene
// 打开场景
EditorApplication.OpenScene("Assets/Launch.unity");
EditorSceneManager.OpenScene("Assets/Launch.unity");
// 编辑器变为运行状态
EditorApplication.isPlaying = true;
```

## 查找资源 物体

```csharp
// 获得/设置选中的gameobject
Selection.gameObjects
Selection.activeGameObject
// 获得选中资源和目录下的资源（如果选中的资源是目录的话，目录也是一种资源Default Asset，基类是UnityEngine.Object）
Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets)
// 返回prefab相对于工程目录的路径（即带有Assets/）
AssetDatabase.GetAssetPath(prefab)
// 返回位于path的主资产，不需要类型，主资产是位于层次结构根目录的资产（例如Maya文件，其中可能包含多个Mesh和GameObjects）
AssetDatabase.LoadMainAssetAtPath(path)
// 返回位于path的资产，因为一个资产可能包含其他多个子资产，因此需要执行类型
AssetDatabase.LoadAssetAtPath<TObject>(path)
// 给定路径，按条件和名称搜索资源 https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html
// 第一个参数填string.Empty则为无条件搜索给定路径
// 返回的资源名称为搜索名称的子集，因此搜索结果与搜索名称并不完全匹配
string[] assetGUIDs = AssetDatabase.FindAssets("t:Prefab prefabName", searchPaths);
// 返回targetTransform相对于root的相对路径
AnimationUtility.CalculateTransformPath(Transform targetTransform, Transform root);
// 指定路径下文件是否存在
bool isExist = UnityEditor.AssetDatabase.AssetPathToGUID(path) != "";
// 是否是文件夹
AssetDatabase.IsValidFolder(path);
// 取得场景下的所有根物体
scene.GetRootGameObjects();
```

## 保存 读取 打开

```csharp
// 将Object保存成资源
AssetDatabase.CreateAsset(asset, path);
// 添加物体到资产上
AssetDatabase.AddObjectToAsset
// 通过默认的Inspector来修改Mono或ScriptableObject对象时，对可序列化对象的这些修改会被自动保存
// 但是如果通过自定义Inspector或代码直接修改序列化字段，则需要手动调用SetDirty告知对象修改需要保存到磁盘
// 如果修改的对象附加在某个prefab上，通过代码（自定义Editor）修改了对象后，要将这些改动保存到prefab上，还需要序列化prefab本身，但是修改这些可序列化对象并不会使得prefab变为dirty（非自定义Editor显示在Inspector上的都是可序列化对象，默认的Editor会提供改动后的SetDirty调用），因此还需要额外调用EditorUtility.SetDirty(serializableComponent);
EditorUtility.SetDirty(obj);
// 将文件改动写入磁盘
AssetDatabase.SaveAssetIfDirty(obj);
// 将所有改动文件写入磁盘
AssetDatabase.SaveAssets();

// 如果在自定义编辑器上修改了serializedObject的一些字段，必须调用该函数才能保存
serializedObject.ApplyModifiedProperties();

// 打开资源，等同于Project视图中双击打开资源，会触发[OnOpenAsset]标签修饰的函数
AssetDatabase.OpenAsset
// 打开资源时的回调，常用于打开ScriptableObject资源时打开对应编辑器窗口
[OnOpenAssetAttribute(priority)]
public static bool OpenAsset(int instanceID, int line)
{
    YourScriptable asset = EditorUtility.InstanceIDToObject(instanceID) as YourScriptable;
    if (asset != null)
    {
        // 打开窗口，使用asset进行初始化
        return true;
    }
    else return false;
}
```

## 文件相关

``` csharp
// 将路径转换为Assets相对路径
string relativePath = FileUtil.GetProjectRelativePath(fullPath);
// 检查路径，如果路径已经存在，则基于原路径创建新的唯一路径
string uniquePath = AssetDatabase.GenerateUniqueAssetPath(originPath);
// 替换文件或文件夹
FileUtil.ReplaceFile
FileUtil.ReplaceDirectory
// 选中并在Project视图中高亮
EditorGUIUtility.PingObject(obj);
// 打开目录
EditorUtility.RevealInFinder(dierctoryPathRelativeToProject);
// 打开文件（用默认应用）
EditorUtility.OpenWithDefaultApp(dierctoryPathRelativeToProject);
// 打开文件对话框
EditorUtility.OpenFilePanel
// 打开文件夹对话框
EditorUtility.OpenFolderPanel
```

## SceneView视图

``` csharp
// Scene变为顶视图
SceneView sceneView = SceneView.lastActiveSceneView;
sceneView.isRotationLocked = true;
sceneView.orthographic = true;
sceneView.rotation = Quaternion.Euler(90f, 0f, 0f);
```

## Scripting Define Symbols 通过代码设置预编译指令

```csharp
private static readonly string[] SteamTestSymbols = new string[] { "STEAMTEST" };

// 添加或删除
if (isEnableSteamTest) AddDefineSymbols(SteamTestSymbols);
else RemoveDefineSymbols(SteamTestSymbols);

private static void AddDefineSymbols(string[] symbols)
{
    string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
    List<string> allDefines = definesString.Split(';').ToList();
    allDefines.AddRange(symbols.Except(allDefines));
    PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        string.Join(";", allDefines.ToArray()));
}

private static void RemoveDefineSymbols(string[] symbols)
{
    string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
    List<string> allDefines = definesString.Split(';').ToList();
    for (int i = symbols.Length - 1; i >= 0; i--)
    {
        allDefines.Remove(symbols[i]);
    }
    PlayerSettings.SetScriptingDefineSymbolsForGroup(
        EditorUserBuildSettings.selectedBuildTargetGroup,
        string.Join(";", allDefines.ToArray()));
}
```

## MenuItem

```cs
using UnityEditor;
using UnityEngine;
public class MenuTest : MonoBehaviour
{
    // 最简单的用法
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("MyMenu/Do Something")]
    static void DoSomething()
    {
        Debug.Log("Doing Something...");
    }    
    
    // Validated menu item.
    // Add a menu item named "Log Selected Transform Name" to MyMenu in the menu bar.
    // We use a second function to validate the menu item
    // so it will only be enabled if we have a transform selected.
    [MenuItem("MyMenu/Log Selected Transform Name")]
    static void LogSelectedTransformName()
    {
        Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
    }    
    
    // 验证：如果返回false则该菜单项不可点击，该函数在显示菜单项之前调用
    // Validate the menu item defined by the function above.
    // The menu item will be disabled if this function returns false.
    [MenuItem("MyMenu/Log Selected Transform Name", true)]
    static bool ValidateLogSelectedTransformName()
    {      
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }
    
    // 快捷键
    // Add a menu item named "Do Something with a Shortcut Key" to MyMenu in the menu bar
    // and give it a shortcut (ctrl-g on Windows, cmd-g on macOS).
    [MenuItem("MyMenu/Do Something with a Shortcut Key %g")]
    static void DoSomethingWithAShortcutKey()
    {
        Debug.Log("Doing something with a Shortcut Key...");
    }
    
    
    // 组件的上下文菜单
    // Add a menu item called "Double Mass" to a Rigidbody's context menu.
    [MenuItem("CONTEXT/Rigidbody/Double Mass")]
    static void DoubleMass(MenuCommand command)
    {
        Rigidbody body = (Rigidbody)command.context;
        body.mass = body.mass * 2;
        Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
    }    
    
    // 新建GameObject
    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarchy context menus.
    [MenuItem("GameObject/MyCategory/Custom Game Object", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
```

**参数priority的差超过20时才会显示分割线**

要创建热键，您可以使用以下特殊字符：**%**（Windows 上为 ctrl，macOS 上为 cmd）、**#**（shift）**和 &**（alt）。如果不需要特殊的修饰键组合，则可以在下划线后给出键。例如，使用热键 shift-alt-g 创建菜单`"MyMenu/Do Something #&g"`。要创建带有热键**g**且未按下任何键修饰符的菜单，请使用`"MyMenu/Do Something _g"`.

参考：https://docs.unity3d.com/ScriptReference/MenuItem.html

## EditMode和PlayMode

有时候进入PlayMode时，想要在新的空场景中执行初始化逻辑，不影响EditMode下的编辑用的场景

```c#
using UnityEditor;

class MyEditorWindow : EditorWindow
{
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    
    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // PlayModeStateChange的四种状态分别对应：即将退出PlayMode/EditMode，已经进入PlayMode/EditMode
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                BeforeStartDebug();
                break;
                // 进入PlayMode
            case PlayModeStateChange.EnteredPlayMode:
                AfterStartDebug();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
                // 进入EditMode
            case PlayModeStateChange.EnteredEditMode:
                AfterStopDebug();
                break;
            default:
                break;
        }
    }
    
    private void BeforeStartDebug()
    {
        // 禁用一些物体上，以避免执行物体上的脚本的Start、OnEnable等函数
        foreach (var controller in targetMono)
        {
            controller.gameObject.SetActive(false);
        }
        // 在回到EditMode时不保留原场景的这些修改
        var debugScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(debugScene);
    }
    
    private void AfterStartDebug()
    { 
		// 在新场景中执行初始化逻辑，避免对原场景的修改
        var newScene = UnityEngine.SceneManagement.SceneManager.CreateScene($"{storyData.name}");
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(newScene);
    }
    
    
    private void AfterStopDebug()
    {
        // 重新打开原场景
       var scene = EditorSceneManager.OpenScene("Assets/Debug.unity"); 
    }
}
```

## 杂项

``` csharp
// 刷新Project视窗（代码创建资源后，Project视图不会自动刷新）
AssetDatabase.Refresh();
// 子物体的transform发生变化
private void OnTransformChildrenChanged()
{
    Debug.Log("Children changed! note: child of children changed won't call this function");
}
// 获得预制体资源的预览图
AssetPreview.GetAssetPreview(prefab);
// 通过InstanceID获得Object，注意该InstanceID会变化，和GUID不同
EditorUtility.InstanceIDToObject

// 是否开启了PlayMode选项，影响ReloaDomain
EditorSettings.enterPlayModeOptionsEnabled
    
// 设置显示菜单项是否被勾选
Menu.SetChecked("MyMenu/IsEnableLog", isEnableLog);

// 获取当前label、设置label、移除所有label
AssetDatabase.GetLabels
AssetDatabase.SetLabels
AssetDatabase.ClearLabels

// 使用label在project窗口搜索资源
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

// 打开对话框
if (EditorUtility.DisplayDialog("Warning", "Are you really sure to delete these assets?", "Yes", "No")) delete something
    
// 弹出对话框询问是否保存已修改的场景：是-》保存修改，返回true，否-》不保存，返回true，取消-》保持现状，返回false
// 如果当前场景没有修改，则不会弹出对话框，且返回true
private static bool HasSaveModifyScene()
{
    bool hasHandleModifySceneOrNoModify = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    if (hasHandleModifySceneOrNoModify) Debug.Log("有修改的场景需要保存，已选择保存或不保存，或没有任何修改");
    else Debug.Log("取消，保持现状");
    return hasHandleModifySceneOrNoModify;
}
```

