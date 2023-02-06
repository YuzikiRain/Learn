## GC

- 缓存对象，避免频繁new。
- 警惕闭包和隐式类型转换造成的装箱拆箱
- 使用NoAlloc版本的API
- 避免频繁实例化、销毁物体：使用对象池

[Unity - Manual: Managed memory (unity3d.com)](https://docs.unity3d.com/Manual/performance-managed-memory.html)

## 物理

- 关闭sync transform

## 自定义Mono脚本

- 避免每一帧都执行：可以额外增加一个间隔时间，使得频率降低。有的逻辑只在某些状态下才执行，或根本只需要执行一次。
- 避免物体自带脚本上的Awake函数做耗时过多的操作。

## 其他

- 避免一帧内实例化过多物体：每一帧只实例化一定预算内的物体
