**世界**是[实体](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-entities.html)的集合。实体的 ID 号仅在其自己的世界中是唯一的。世界具有[`EntityManager`](https://docs.unity3d.com/Packages/com.unity.entities@1.0/api/Unity.Entities.EntityManager.html)结构，您可以使用该结构创建、销毁和修改世界中的实体。

一个世界拥有一组系统，这些[系统](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-systems.html)通常只访问同一世界中的实体。此外，世界中具有相同组件类型的一组实体一起存储在[Archetype](https://docs.unity3d.com/Packages/com.unity.entities@1.0/manual/concepts-archetypes.html)中，这决定了程序中的组件在内存中的组织方式。

一般在System的OnUpdate等方法中通过`systemState.EntityManager`来访问system所在world的EntityManager。