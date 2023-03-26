# IComponentData

对应ECS中的C，但不等同于传统的Mono脚本组件。

``` c#
using Unity.Entities;
using Unity.Mathematics;

public struct MyComponentDataProperties : IComponentData
{
    public float2 vector2;
    public int number;
    public Entity entityPrefab;
}
```

# Baker

传统的Mono脚本保存在GameObject上，以GameObject为单位，因此可以方便地在Inspector下编辑对应的字段。但对于一个GameObject来说，ECS的C即IComponentData是按ComponentData类型分组的，没法在编辑器上编辑，需要一个**转换器**即**Baker**将传统组件转换成实体组件数据。

``` c#
public MyMono
{
    public float2 vector2;
    public int number;
    public GameObject entityPrefab;
}

public class MyComponentBaker : Baker<MyMono>
{
    public override void Bake(MyMono ahthoring)
    {
        AddComponent(new MyComponentDataProperties()
         {
             vector2 = authoring.vector2,
             number = authoring.number,
             entityPrefab = GetEntity(authoring.entityPrefab),
         });
        // 如果有多个组件，再继续进行转换
        ...
    }
}
```





