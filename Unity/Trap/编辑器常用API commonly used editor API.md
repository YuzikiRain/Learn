```csharp
        // 预制体，且不在场景中 is a prefab in asset
        var IsPartOfPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(gameObject);
        // 预制体，且在场景中   is a prefab in hierarchy(scene) as instance
        var IsPartOfPrefabInstance = PrefabUtility.IsPartOfPrefabInstance(gameObject);
```