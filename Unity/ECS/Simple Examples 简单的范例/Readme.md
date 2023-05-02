# 环境

Unity2022.2.12f1、插件Entities 1.0.0-pre.65

参考自官方范例工程：[Unity-Technologies/EntityComponentSystemSamples (github.com)](https://github.com/Unity-Technologies/EntityComponentSystemSamples)，选择master或者release/samples-1.0-exp

# 吐槽

现在是2023年4月，国内的很多unity ecs教程鱼龙混杂，要么就是1.0以前的版本，api已经过时了；要么就是用的，然后让你用一个已经不存在的Aspect类`TransformAspect`来实现旋转（如下）。我找了半天都不知道这个该咋个弄，真的是把我坑惨了。在Entities 1.0.0-pre.65下要改成用LocalTransform

``` c#
[BurstCompile]
public void OnUpdate(ref SystemState state)
{
    var rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);
    foreach (var transform in SystemAPI.Query<TransformAspect>())
    {
        transform.RotateWorld(rotation);
    }
}
```

建议大家不要上来就找那种三无教程，或者是搬运老外的视频教程，很多连插件的版本号都不给你说清楚，你照着写经常就是编译错误或者是运行了一直报错，人都要疯了。

这里只推荐一种，就是找unity官方在github上的ecs example，下下来打开直接就能跑。循序渐进，一个个例子看完，很快就能理解。

