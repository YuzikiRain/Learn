- 创建资产

    - Asset.Create

        ``` csharp
        var asset = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(asset, "Assets/MyMaterial.mat");
        ```

    - ScriptableObject.CreateInstance

        ``` csharp
        var asset = ScriptableObject.CreateInstance<TimelineAsset>();
        AssetDatabase.CreateAsset(asset, "Assets/MyMaterial.mat");
        ```

- 序列化到硬盘中

```csharp
// create some asset
// call SetDirty before call SaveAssets, or the asset you just create will not be marked dirty, and SaveAssets won't work
EditorUtility.SetDirty(asset);
AssetDatabase.SaveAssets();
```

