``` csharp
using System;
using UnityEditor;

class MyAllPostprocessor : AssetPostprocessor
{
    public static event Action OnAssetsChanged;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        OnAssetsChanged?.Invoke();
    }
}
```

