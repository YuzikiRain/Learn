# IComponentData

对应ECS中的C，不等同于传统的Mono脚本组件。

它应当只包含**非托管类型**数据，最好不包含方法。System可以对这些数据进行读写。

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
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MyMono : MonoBehaviour
{
    public float2 vector2;
    public int number;
    public GameObject entityPrefab;

    public class MyComponentBaker : Baker<MyMono>
    {
        public override void Bake(MyMono authoring)
        {
            AddComponent(new MyComponentDataProperties()
            {
                vector2 = authoring.vector2,
                number = authoring.number,
                entityPrefab = GetEntity(authoring.entityPrefab),
            });
            // 如果有多个组件，再继续进行转换
            //...
        }
    }
}
```

## 在Entity中对Component进行修改

## 向Entity添加、删除Component

若要向实体添加组件，请对实体所在的[世界](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-worlds.html)使用[`实体管理器`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityManager.html)。您可以将组件添加到单个实体，也可以同时添加到多个实体。

向实体添加、删除组件是一种[结构更改](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-structural-changes.html)，这意味着实体移动到不同的块。这意味着您**无法直接从jobs向实体添加、删除组件**。相反，您必须使用 [`EntityCommandBuffer`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityCommandBuffer.html) 来记录您以后添加、删除组件的意图。

### 添加单个

```cs
public partial struct AddComponentToSingleEntitySystemExample : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponent<Rotation>(entity);
    }
}
```

### 添加多个

```cs
struct ComponentA : IComponentData {}
struct ComponentB : IComponentData {}
public partial struct AddComponentToMultipleEntitiesSystemExample : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var query = state.GetEntityQuery(typeof(ComponentA));
        state.EntityManager.AddComponent<ComponentB>(query);
    }
}
```

### 删除单个、多个

```cs
public partial struct RemoveComponentSystemExample : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var query = state.GetEntityQuery(typeof(Rotation));
        state.EntityManager.RemoveComponent<Rotation>(query);
    }
}
```

## 读写Component

Component指的是实现IComponentData接口的struct，由于被保存在chunk的组件数组中，只能通过直接替换整个struct本身来达到修改的目的。（因为对struct数组读取元素只会返回元素的拷贝，无法修改到数组上的元素的值）

在主线程上，可以通过`EntityManager.GetComponentData`或`EntityManager.SetComponentData`来立即读写Entity的Component。

在jobs上，可通过EntityCommandBuffer来记录写入Component的操作，这些更改仅在稍后在主线程上播放EntityCommandBuffer时发生。
