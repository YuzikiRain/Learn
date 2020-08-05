```csharp
	// 预制体，且不在场景中 is a prefab in asset
	var IsPartOfPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(gameObject);
	// 预制体，且在场景中   is a prefab in hierarchy(scene) as instance
	var IsPartOfPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(gameObject);
    private void OnTransformChildrenChanged()
    {
        Debug.Log("Children changed! note: child of children changed won't call this function");
    }
	// 请求刷新布局
	LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
	// 强制刷新布局
	LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
```