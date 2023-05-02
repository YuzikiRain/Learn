# ISystem

## 生命周期

![img](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/images/SystemEventOrder.png)

必选：

| **方法**                                                     | **描述**                                           |
| :----------------------------------------------------------- | :------------------------------------------------- |
| [`OnCreate`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ISystem.OnCreate.html) | 系统事件回调，用于在使用前初始化系统及其数据。     |
| [`OnUpdate`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ISystem.OnUpdate.html) | 系统事件回调，用于添加系统必须执行的每一帧的工作。 |
| [`OnDestroy`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ISystem.OnDestroy.html) | 系统事件回调，用于在销毁之前清理资源。             |

可选：

| **方法**                                                     | **描述**                                                     |
| :----------------------------------------------------------- | :----------------------------------------------------------- |
| [`OnStartRunning`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ISystemStartStop.OnStartRunning.html) | 在第一次调用`OnUpdate`之前以及系统停止或禁用后恢复时进行系统事件回调。 |
| [`OnStopRunning`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.ISystemStartStop.OnStopRunning.html) | 系统被禁用或与系统更新所需的任何组件不匹配时的系统事件回调。 |

[ISystem overview | Entities | 1.0.0-pre.65 (unity3d.com)](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-isystem.html)

## Require

可以在OnCreate等方法中对System进行约束，仅当world中存在对应的IComponentData时，才运行该System

```c#
public void OnCreate(ref SystemState state)
{
    state.RequireForUpdate<YourIComponentData>();
}
```

# 系统组 SystemGroup

### 系统创建顺序

默认情况下，Unity在创建system时不关心system group，而关心`CreateAfter`和`CreateBefore`标签

`OnCreate`方法的调用顺序与创建系统的顺序相同。

最佳做法是使用 [`CreateAfter`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.CreateAfterAttribute.html) 和 [`CreateBefore`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.CreateBeforeAttribute.html) 属性，以及默认世界创建或 [`ICustomBootstrap`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-icustombootstrap.html) 用于高级用例。这是通过调用[`World.GetOrCreateSystem`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.World.CreateSystem.html)创建系统的推荐方法。

这降低了出错的可能性，例如，如果系统创建顺序更改为另一个也满足属性约束的顺序。如果必须在`OnCreate`方法中引用另一个系统，则应使用`CreateAfter`确保在引用的另一个系统之后创建系统，然后使用 [`World.GetExistingSystem`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.World.GetExistingSystem.html) 检索它。

### 系统销毁顺序

当您调用 [`World.Dispose`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.World.Dispose.html#Unity_Entities_World_Dispose) 时，Unity 会按照与实际创建的相反顺序销毁系统。Unity 会按此顺序销毁它们，即使它们的创建违反了`CreateAfter` or `CreateBefore`约束（例如，如果您手动调用`CreateSystem` out of order）。

### 系统更新顺序

每次将组添加到系统组时，该组都会重新排序该组的系统更新顺序，然后再进行更新。若要控制系统组的更新顺序，请将“更新之前”或“更新之后”属性添加到系统中，以指定在系统[`之前`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.UpdateBeforeAttribute.html)或[`之后应更新`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.UpdateAfterAttribute.html)的系统。这些属性仅适用于同一系统组的直接子级。

您还可以使用 [`OrderFirst`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.UpdateInGroupAttribute.OrderFirst.html) 或 [`OrderLast`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.UpdateInGroupAttribute.OrderLast.html) 参数自定义更新顺序，这些参数优先于`UpdateBefore`和`UpdateAfter` 。例如：

```c#
// If PeachSystem and DaisySystem are children of the same group, then the
// entity component system puts PeachSystem somewhere before DaisySystem in 
// the sorted order.
[UpdateBefore(typeof(DaisySystem))]
public partial class PeachSystem : SystemBase { }
```

由于组的更新列表中的所有内容一起更新，因此系统更新的全局顺序表示分层顺序，其中组的所有直接子级首先按 `OrderFirst`和`OrderLast`排序，然后按`UpdateBefore`和`UpdateAfter`约束排序。由于子组也可以是系统组，因此这些子组可能会在返回到当前组的更新列表之前更新许多孙子系统。

### 默认system group

有一组默认系统组可用于在帧正确的阶段更新system。您可以将一个系统组嵌套在另一个系统组中，以便组中的所有系统都在正确的阶段更新，并根据其组内的顺序进行更新。

默认世界包含实例层次结构。Unity 播放器循环中有三个根级系统组：`ComponentSystemGroup`

-   **初始化系统组**：在player循环的`Initialization`阶段结束时更新。
-   **模拟系统组**：在player循环`Update`阶段结束时进行更新。
-   **演示系统组**：在player循环`PreLateUpdate`阶段结束时更新。

### System order

可以在系统上使用以下属性来确定其更新顺序：

| **属性**                     | **描述**                                                     |
| :--------------------------- | :----------------------------------------------------------- |
| `UpdateInGroup`              | 指定此系统应属于哪个`ComponentSystemGroup` 。如果未设置此属性，它将自动添加到默认世界的`SimulationSystemGroup` .<br />您可以使用可选的`OrderFirst`和`OrderLast`参数，将其排序到组中没有相同参数集的**所有其他系统的之前或之后**。 |
| `UpdateBefore` `UpdateAfter` | 相对于同一组中的其他系统对系统进行排序。<br />将此属性应用于组时，它们会隐式约束其中的所有成员系统。  <br />例如，如果`CarSystem`在`CarGroup`且`TruckSystem`在`TruckGroup`，`CarGroup`和`TruckGroup`都是`VehicleGroup`的成员，则`CarGroup`和`TruckGroup`的排序隐式决定了`CarSystem`和`TruckSystem`的相对顺序（比如A组在B组之前，则A组的a必然在B组的b之前）。 |
| `CreateBefore` `CreateAfter` | 排序了相对于其他系统的创建。相同的排序规则适用于`UpdateBefore`和`UpdateAfter` ，只是没有组，也没有`OrderFirst`或`OrderLast`。这些属性将覆盖默认行为。系统销毁顺序定义为与创建顺序相反的顺序。 |
| `DisableAutoCreation`        | 阻止 Unity 在默认世界初始化期间创建系统。详见[Manual system creation](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-update-order.html#manual-system-creation) 。 |

[System groups | Entities | 1.0.0-pre.65 (unity3d.com)](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-update-order.html)

## 常见错误

``` c#
// 错误：少了ref关键字
public void OnCreate(SystemState state) {}

// 错误：应该是SystemState
public void OnCreate(SystemBase state) {}
```

# Entity Command Buffer 实体命令缓冲区

## 概述

[`EntityCommandBuffer` 中的方法](xref:Unity.Entities.EntityCommandBuffer.)记录命令，这些命令镜像 [`EntityManager`](xref:Unity.Entities.EntityManager.html) 中可用的方法。例如：

-   `CreateEntity(EntityArchetype)`：注册使用指定原型创建新实体的命令。
-   `DestroyEntity(Entity)`：注册销毁实体的命令。
-   `SetComponent<T>(Entity, T)`：注册一个命令，该命令为实体上的类型组件`T`设置值。
-   `AddComponent<T>(Entity)`：注册将类型组件`T`添加到实体的命令。
-   `RemoveComponent<T>(EntityQuery)`：注册从与查询匹配的所有实体中删除类型组件`T`的命令。

## EntityCommandBuffer和EntityManager的比较

-   如果要将作业中的结构更改排队，[则必须使用 ECB](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/systems-entity-command-buffer-use.html)。
-   如果要在主线程上执行结构更改并立即执行这些更改，请使用`EntityManager`中的方法。
-   如果要在主线程上执行结构更改，并且希望在以后（例如作业完成后）进行结构更改，则应使用 ECB。

**ECB 中记录的更改仅在主线程上调用`Playback`时应用**。如果您尝试在播放后记录对 ECB 的任何进一步更改，Unity 会引发异常。