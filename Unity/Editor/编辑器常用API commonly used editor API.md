### Prefab

``` csharp
// 预制体，且不在场景中（在Asset中，是资源） is a prefab in asset
var IsPartOfPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(gameObject);
// 预制体，且在场景中   is a prefab in hierarchy(scene) as instance
var IsPartOfPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(gameObject);
// 从prefabInstance的子物体中取得prefab的root
var prefabInstanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(prefabInstance)
// 从Object获得prefab，应配合 PrefabUtility.GetOutermostPrefabInstanceRoot 使用，因为如果参数不是 prefabInstanceRoot 会返回 null
var prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(prefabInstanceRoot);
var prefabAssetPath = AssetDatabase.GetAssetPath(prefabAsset);
// 如果参数 prefabInstanceRoot 确实是prefab的根物体，返回 null 表示在预制体模式下，否则在一般场景下
bool isInPrefabStage = prefabInstance != null;

// 用预制体资源创建预制体实例，并保持与预制体资源连接
PrefabUtility.InstantiatePrefab(prefab);
// 获得预制体场景下的根物体，即预制体根物体本身
root = prefabStage.prefabContentsRoot;
// 使用实例创建prefab并连接
PrefabUtility.SaveAsPrefabAssetAndConnect
PrefabUtility.SaveAsPrefabAsset
```

### Stage

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

### EditorScene

```csharp
// 编辑器模式下获得当前激活场景
EditorSceneManager.GetActiveScene();
// 编辑器模式下关闭场景
EditorSceneManager.CloseScene
// 编辑器模式下关闭PreviewScene场景（不可用于PrefabStage场景）
EditorSceneManager.ClosePreviewScene
```

### 查找资源 物体

```csharp
// 获得/设置选中的gameobject
Selection.gameObjects
Selection.activeGameObject
// 返回位于path的主资产，不需要类型，主资产是位于层次结构根目录的资产（例如Maya文件，其中可能包含多个Mesh和GameObjects）
AssetDatabase.LoadMainAssetAtPath(path)
// 返回位于path的资产，因为一个资产可能包含其他多个子资产，因此需要执行类型
AssetDatabase.LoadAssetAtPath<TObject>(path)
// 给定路径，按条件和名称搜索资源
string[] assetGUIDs = AssetDatabase.FindAssets("t:Prefab prefabName", searchPaths);
// 刷新Project视窗（代码创建资源后，Project视图不会自动刷新）
AssetDatabase.Refresh

```

### 保存 读取 打开

```csharp
// 将Object保存成资源
AssetDatabase.AddObjectToAsset
// 通过默认的Inspector来修改Mono或ScriptableObject对象时，这些修改会被自动保存
// 但是如果通过自定义Inspector或代码直接修改，则需要手动调用SetDirty告知对象修改需要保存到磁盘
EditorUtility.SetDirty(obj);
// 将改动写入磁盘
AssetDatabase.SaveAssets();
// 打开资源，等同于Project视图中双击打开资源，会触发[OnOpenAsset]标签修饰的函数
AssetDatabase.OpenAsset
```

### 文件相关

``` csharp
// 将路径转换为Assets相对路径
string relativePath = FileUtil.GetProjectRelativePath(fullPath);
// 替换文件或文件夹
FileUtil.ReplaceFile
FileUtil.ReplaceDirectory
// 选中并在Project视图中高亮
EditorGUIUtility.PingObject(obj);
// 打开目录
EditorUtility.RevealInFinder(dierctoryPathRelativeToProject);
EditorUtility.OpenWithDefaultApp(dierctoryPathRelativeToProject);
```

### Attribute

``` csharp
// 打开资源时的回调，常用于打开ScriptableObject资源时打开对应编辑器窗口
[OnOpenAsset]
```

### SceneView视图

``` csharp
// Scene变为顶视图
SceneView sceneView = SceneView.lastActiveSceneView;
sceneView.isRotationLocked = true;
sceneView.orthographic = true;
sceneView.rotation = Quaternion.Euler(90f, 0f, 0f);
```

### 杂项

``` csharp
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
```

