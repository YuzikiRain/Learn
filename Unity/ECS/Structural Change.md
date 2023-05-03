导致 Unity 重新组织内存块或[内存中块](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-archetypes.html#archetype-chunks)内容的操作称为**结构更改**。了解哪些操作是结构更改非常重要，因为它们可能是资源密集型的，并且只能在主线程上执行它们，而不是在工作线程上。

以下操作被视为结构更改：

- 创建或销毁[实体](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-entities.html)。
- 添加或删除[组件](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-components.html)。
- 设置shared component的值。

## 创建实体

创建实体时，Unity 会将实体添加到现有区块中，如果没有可用于实体原型的[区块，则](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-archetypes.html)创建一个新区块并将实体添加到该区块中。

## 销毁实体

销毁实体时，Unity 会从其区块中删除该实体。如果删除实体会在区块中留下间隙，Unity 会移动区块中的最后一个实体以填充空白。如果删除实体会将区块留空，Unity 将解除分配区块。

## 添加或删除组件

在实体中添加或删除组件时，可以更改实体的原型。Unity 将每个实体存储在与实体原型匹配的块中。这意味着，如果更改实体的原型，Unity 必须将实体移动到另一个区块。如果不存在合适的区块，Unity 会创建一个新区块。如果移动使前一个区块有间隙或将其留空，Unity 将移动块中的最后一个实体以填充间隙或分别解除分配块。

## 设置共享组件值

设置实体共享[组件](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/components-shared.html)的值时，Unity 会将实体移动到与新的共享组件值匹配的区块。如果不存在合适的区块，Unity 会创建一个新区块。如果移动使前一个块有间隙或空白，Unity 将移动块中的最后一个实体以填充间隙或分别解除分配块。

> ##### 注意
>
> 设置常规组件值不是结构更改，因为它不需要 Unity 来移动实体。

## 同步点（Sync Points）

不能直接在jobs中进行结构更改，因为它可能会使已计划的其他作业失效，并创建[同步点](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-structural-changes.html#sync-points)。

同步点（sync point）是程序执行中的一个点，用于等待到目前为止已计划的所有作业的完成。同步点限制您在一段时间内使用作业系统中可用的所有工作线程的能力。因此，您应该避免同步点。ECS 中数据的结构变化是同步点的主要原因。

结构更改不仅需要同步点，而且还会使对任何组件数据的所有直接引用无效。这包括 [`DynamicBuffer`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.DynamicBuffer-1.html) 的实例和提供对组件（如 [`ComponentSystemBase.GetComponentDataFromEntity`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ComponentSystemBase.GetComponentDataFromEntity.html)）的直接访问的方法的结果。

### 避免同步点

可以使用[实体命令缓冲区](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-entity-command-buffers.html)对结构更改进行排队，而不是立即执行它们。您可以在帧期间的稍后时间点播放存储在实体命令缓冲区中的命令。这会将分布在帧中的多个同步点减少到单个同步点。

每个标准[`ComponentSystemGroup`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ComponentSystemGroup.html)实例都提供一个 [`EntityCommandBufferSystem`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityCommandBuffer.html) 作为组中更新的第一个和最后一个系统。如果从这些标准系统之一获取实体命令缓冲区对象，则所有结构更改都发生在帧中的同一点，从而生成一个同步点。您还可以使用实体命令缓冲区来记录作业中的结构更改，而不是仅在主线程上进行结构更改。

如果不能对任务使用实体命令缓冲区，请按系统执行顺序将进行结构更改的任何系统分组在一起。两个都进行结构更改的系统在按顺序更新时仅创建一个同步点。