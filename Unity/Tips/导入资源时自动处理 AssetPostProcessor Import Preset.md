### Default Preset

-   Project Settings -> Preset Manager -> Add Default Preset -> Importer，选择对应资源，然后进行设置
-   Filter 可以筛选要使用该Importer的资源

### AssetPostProcessor

继承自AssetPstprocessor并编写对应逻辑

``` csharp
using UnityEngine;
using UnityEditor;

// Automatically convert any texture file with "_bumpmap"
// in its file name into a normal map.

class MyTexturePostprocessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (assetPath.Contains("_bumpmap"))
        {
            TextureImporter textureImporter  = (TextureImporter)assetImporter;
            textureImporter.convertToNormalmap = true;
        }
    }
}
```

参考

-   https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html
-   https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html

