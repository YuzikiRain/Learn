```csharp
// create some asset
// call SetDirty before call SaveAssets, or the asset you just create will not be marked dirty, and SaveAssets won't work
EditorUtility.SetDirty(asset);
AssetDatabase.SaveAssets();
```

