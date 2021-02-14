如果设置了一些脚本的值，PrefabStage的AutoSave并不会工作，需要手动保存

```csharp
var prefabInstance = PrefabUtility.GetCorrespondingObjectFromSource(obj);
// 非打开预制体模式下
if (prefabInstance)
{
    root = PrefabUtility.GetOutermostPrefabInstanceRoot(selectedObj);
    var prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(root);
    prefabPath = AssetDatabase.GetAssetPath(prefabAsset);
    // 修改预制体，则只能先Unpack预制体再保存
    PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.UserAction);
    // 做一些修改
    // 保存并连接
    PrefabUtility.SaveAsPrefabAssetAndConnect(obj, prefabPath, InteractionMode.AutomatedAction);
    // 不修改只新增，可以直接保存
    PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
}
else
{
    // 预制体模式下，从Prefab场景中取得预制体资源位置和根物体，并保存
    PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
    string path = prefabStage.prefabAssetPath;
    // 修改，需要指定路径保存
    GameObject root = prefabStage.prefabContentsRoot;
    PrefabUtility.SaveAsPrefabAsset(root, path);
    // 新增，可以直接用预制体引用保存
    PrefabUtility.SavePrefabAsset(root);
}
```

如果在```OnValidate```中，则无法调用```PrefabUtility.SaveAsPrefabAsset``` 或 ```PrefabUtility.SavePrefabAsset``` 进行保存，此时可以使用 ```UnityEditor.EditorUtility.SetDirty(gameObject)``` 将修改的部分设为脏以触发自动保存

