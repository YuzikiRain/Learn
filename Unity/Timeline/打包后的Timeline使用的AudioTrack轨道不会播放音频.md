代码剥离问题

`Project Settings->Player->Other Setting->Configuration->Scripting Backend`选择了IL2CPP时，`Project Settings->Player->Other Setting->Optimization->Managed Stripping Level`不能选择Disable（至少要选择Low），而如果选择了Mono，则可以选择Disable

## 解决方案

- 新建一个预制体，添加PlayableDirector脚本，引用任意一个TimelineAsset资源（最好是空的，不会用到的），将该预制体放到`Resources`目录下
- link.xml：参考[Unity - Manual: Managed code stripping (unity3d.com)](https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
- `Project Settings->Player->Other Setting->Configuration->Scripting Backend`使用Mono而不是IL2CPP，这样`Project Settings->Player->Other Setting->Optimization->Managed Stripping Level`就能选择Disable了，这样Build时就不会剥离任何代码
- 使用2021.3版本，同上，不同之处是`Project Settings->Player->Other Setting->Optimization->Managed Stripping Level`最小可以选择**Minimal**（默认值）