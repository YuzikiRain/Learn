## 现象
- 将Sprite拖放到Sprite Atlas资源的Objects for Packing中，点击Pack Preview并保存
- 该Sprite被某个prefab的SpriteRenderer所引用，使用如下代码加载prefab，Editor模式下能正常显示，并减少了DrawCall，直接用Sprite拖放创建的图片也能显示

  ``` csharp
  string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
  return AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
  ```

- 将Sprite从Sprite Atlas资源的Objects for Packing中移除，点击Pack Preview并保存
- Editor模式下prefab和使用Sprite拖放创建的图片都不显示Sprite，但SpriteRenderer组件仍引用了正确的Sprite资源
- **Editor模式下再次运行又能正常显示**
## 原因
可能是一开始在添加到Atlas，再从Atlas中移除后，首次运行仍去加载图集中对应的Sprite，再次运行时则是加载被移除出图集的Sprite
