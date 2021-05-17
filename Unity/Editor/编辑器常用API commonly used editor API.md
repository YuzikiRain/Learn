### Editor

```csharp
// 获得选中的gameobject
Selection.gameObjects
Selection.activeGameObject
// 返回位于path的主资产，不需要类型，主资产是位于层次结构根目录的资产（例如Maya文件，其中可能包含多个Mesh和GameObjects）
AssetDatabase.LoadMainAssetAtPath(path)
// 返回位于path的资产，因为一个资产可能包含其他多个子资产，因此需要执行类型
AssetDatabase.LoadAssetAtPath(path)
// 预制体，且不在场景中（在Asset中，是资源） is a prefab in asset
var IsPartOfPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(gameObject);
// 预制体，且在场景中   is a prefab in hierarchy(scene) as instance
var IsPartOfPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(gameObject);
// 从prefabInstance的子物体中取得prefab的root
var prefabInstanceRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(prefabInstance)
// 从Object获得prefab，应配合 PrefabUtility.GetOutermostPrefabInstanceRoot 使用，因为如果参数不是 prefabInstanceRoot 会返回 null
var prefabInstance = PrefabUtility.GetCorrespondingObjectFromSource(prefabInstanceRoot);
// 如果参数 prefabInstanceRoot 确实是prefab的根物体，返回 null 表示在预制体模式下，否则在一般场景下
bool isInPrefabStage = prefabInstance != null;
// 在预制体模式下，获得预制体场景
PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
// 获得预制体场景下的根物体，即预制体根物体本身
root = prefabStage.prefabContentsRoot;
// 使用实例创建prefab并连接
PrefabUtility.SaveAsPrefabAssetAndConnect
PrefabUtility.SaveAsPrefabAsset
// 将Object保存成资源
AssetDatabase.AddObjectToAsset
// 通过默认的Inspector来修改Mono或ScriptableObject对象时，这些修改会被自动保存
// 但是如果通过自定义Inspector或代码直接修改，则需要手动调用SetDirty告知对象修改需要保存到磁盘
EditorUtility.SetDirty(obj);
// 将改动写入磁盘
AssetDatabase.SaveAssets();
    
// 刷新Project视窗
UnityEditor.AssetDatabase.Refresh
// 替换文件或文件夹
UnityEditor.FileUtil.ReplaceFile
UnityEditor.FileUtil.ReplaceDirectory
// 选中并在Project视图中高亮
EditorGUIUtility.PingObject(obj);
// 打开目录
EditorUtility.RevealInFinder(dierctoryPathRelativeToProject);
EditorUtility.OpenWithDefaultApp(dierctoryPathRelativeToProject);

// 子物体的transform发生变化
private void OnTransformChildrenChanged()
{
    Debug.Log("Children changed! note: child of children changed won't call this function");
}
// 通过InstanceID获得Object，主要用于序列化
EditorUtility.InstanceIDToObject
// 检查编辑器上的属性是否被修改
EditorGUI.BeginChangeCheck();
some code may change editor
if (EditorGUI.EndChangeCheck())
{
    SetKeyword("_METALLIC_MAP", map.textureValue);
}
// 是否开启了PlayMode选项，影响ReloaDomain
EditorSettings.enterPlayModeOptionsEnabled
```

### FileUtil

``` csharp
// 将路径转换为Assets相对路径
string relativePath = FileUtil.GetProjectRelativePath(fullPath);
```

