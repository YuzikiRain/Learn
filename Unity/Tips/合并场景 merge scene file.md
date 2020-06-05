- 安装BeyondCompare
- 打开SourceTree，Tool > Options > Diff
    - MergeTool：自定义
    - Merge Commamd：Unity编辑器下的UnityYAMLMerge.exe所在路径，如果是安装在C盘默认位置则是```C:\Program Files (x86)\Unity\Editor\Data\Tools\UnityYAMLMerge.exe```，
    - Arguments填 ```merge -p $BASE $REMOTE $LOCAL $MERGED```
- 如果BeyondCompare没有安装在默认路径下，找到UnityYAMLMerge.exe同一目录下的mergespecfile.txt文件，在BeyondComapre下增加一行```* use "your BeyondCompare fullpath" "%r" "%l" "%b" "%d"```

### 参考
https://docs.unity3d.com/Manual/SmartMerge.html?_ga=2.217909910.1000782007.1591077862-1573029967.1590940215
