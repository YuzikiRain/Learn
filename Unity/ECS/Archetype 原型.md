**原型**是[world](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-worlds.html)中具有相同唯一[组件](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-components.html)类型组合的所有[实体](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-entities.html)的唯一标识符。

在实体中添加或删除组件类型时，世界的[`EntityManager`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityManager.html)会将实体移动到相应的原型。例如，如果实体具有组件类型 A、B 和 C，并且您删除了其 B 组件，则`EntityManager`会将该实体移动到具有组件类型 A 和 C 的原型。如果不存在这样的原型，则`EntityManager`创建它。

基于原型的实体组织意味着按组件类型查询实体非常有效。例如，如果要查找具有组件类型 A 和 B 的所有实体，则可以查找具有这些组件类型的所有原型，这比扫描所有单个实体的性能更高。世界中的现有原型集往往会在程序生存期的早期稳定下来，因此您可以缓存查询以获得更快的性能。

一个原型只有在它的世界被摧毁时才会被摧毁。

# Archetype Chunk

具有相同原型的所有实体和组件都存储在称为**chunks**的统一内存块（uniform blocks of memory）中。每个块由 16KiB 组成，它们可以存储的实体数量取决于块原型中组件的数量和大小。[`EntityManager`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityManager.html)根据需要创建和销毁块。

chunk包含每个组件类型的数组，以及用于存储实体 ID 的附加数组。例如，在组件类型 A 和 B 的原型中，每个区块有三个数组：一个数组用于 A 组件值，一个数组用于 B 组件值，一个数组用于实体 ID。

chunk数组是**紧密打包**的：块的第一个实体存储在这些数组的索引 0 处，块的第二个实体存储在索引 1 处，后续实体存储在连续索引中。将新实体添加到区块时，它将存储在第一个可用索引中。当从块中删除实体时（因为它被销毁或移动到另一个原型），块的最后一个实体将被移动以填充空白。

chunk数组会**自动伸缩**：`EntityManager`将实体添加到原型时，如果原型的现有块全部已满，则会创建一个新块。当最后一个实体从块中删除时，`EntityManager`将销毁该块。