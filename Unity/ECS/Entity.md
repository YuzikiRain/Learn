**实体**表示程序中具有自己的数据集的离散内容，实体充当将各个唯一组件关联在一起的**ID**，而不是包含任何代码或充当其关联[组件](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-components.html)的容器。

实体集合存储在world中，[`world`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.World.html)的`EntityManager`管理世界中的所有[`实体`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityManager.html)。 `EntityManager`包含可用于创建、销毁和修改该世界中的实体的方法。这些包括以下常用方法：

| **方法**          | **描述**                           |
| :---------------- | :--------------------------------- |
| `CreateEntity`    | 创建新实体。                       |
| `Instantiate`     | 复制现有实体并从该副本创建新实体。 |
| `DestroyEntity`   | 销毁现有实体。                     |
| `AddComponent`    | 将组件添加到现有实体。             |
| `RemoveComponent` | 从现有实体中删除组件。             |
| `GetComponent`    | 检索实体组件的值。                 |
| `SetComponent`    | 覆盖实体组件的值。                 |

> ##### 注意
>
> 创建或销毁实体时，这是一种**结构更改**，会影响应用程序的性能。有关更多信息，请参阅有关[结构更改](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-structural-changes.html)的文档

