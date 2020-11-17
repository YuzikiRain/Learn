### 环境

-   使用SkeletonUtility添加SkeletonUtilityBone组件，其中某些设置Mode为Override（覆盖原动画对骨骼的修改）
-   添加DynamicBone组件来修改Mode为Override的骨骼物体
-   在Timeline里添加Spine动画Track，Director.stopped事件触发时播放其他Timeline（其中也添加了Spine动画Track）

### 修改源码

SpineAnimationStateMixerBehaviour.cs

``` csharp
// Ensure that the first frame ends with an updated mesh.
if (skeletonAnimation)
{
    //skeletonAnimation.Update(0);
    //skeletonAnimation.LateUpdate();
}
```

