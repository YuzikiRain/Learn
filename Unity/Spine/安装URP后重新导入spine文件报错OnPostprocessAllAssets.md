重新导入spine文件以及文件夹下的material、dataAsset、atlasDataAsset等，会报错OnPostprocessAllAssets

### 解决方法

- 重新安装URP插件
- 如果还有报错则需要打开Packages文件夹，手动将json配置中的urp条目删除以触发重新编译