```csharp
            var prefabInstance = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            // 非打开预制体模式下
            if (prefabInstance)
            {
                var prefabPath = AssetDatabase.GetAssetPath(prefabInstance);
                // 修改预制体，则只能先Unpack预制体再保存
                PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.UserAction);
                PrefabUtility.SaveAsPrefabAssetAndConnect(obj, prefabPath, InteractionMode.AutomatedAction);
                // 不修改只新增，可以直接保存
                PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
            }
            else
            {
                // 预制体模式下，从Prefab场景中取得预制体资源位置和根物体，并保存
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                string path = prefabStage.prefabAssetPath;
                GameObject root = prefabStage.prefabContentsRoot;
                PrefabUtility.SaveAsPrefabAsset(root, path);
            }
```

