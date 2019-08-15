利用DynamicBones组件使Spine的头发飘动，事先附加好组件DynamicBone和Spine的SkeletonUtilityBone，并将头发的所有骨骼的Mode设置为Override

### 现象
- 当DynamicBone组件一开始就是enable时，运行，DynamicBones没有效果，但将DynamicBones组件disble再enable后又有效了
- 当DynamicBone组件一开始就是disable时，运行，然后再enable该组件，有效

### 原因
可能是SkeletonUtilityBone组件的运行顺序与DynamicBone组件有冲突

### 解决方法
找到DynamicBone.cs脚本，将以下代码
```csharp
void Start()
{
    SetupParticles();
}
```
改为
```csharp
void Start()
{
    // to fix error: doen't work for spine bone, only works after this component enable
    this.enabled = false;
    this.enabled = true;
    SetupParticles();
}
```
