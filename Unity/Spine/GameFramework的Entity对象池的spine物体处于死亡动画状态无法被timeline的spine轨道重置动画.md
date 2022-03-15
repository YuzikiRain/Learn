```c#
// GameFramework的Entity对象池缓存了spine物体的状态（比如死亡动画状态），无法通过播放Timeline来重置，因此这里需要再次初始化
skeletonAnimation.Initialize(true)
```