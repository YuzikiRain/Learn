``` c#
Camera.main.ScreenPointToRay(Input.mousePosition);
int hitInfosLength = Physics.RaycastNonAlloc(ray, hitInfos, 999f);
// hitInfos的前hitInfosLength个hitInfo才是有效的，其他都不需要访问
for (int i = 0; i < hitInfosLength; i++)
{
    var hitInfo = hitInfos[i];
}
```

## hitInfos是未排序的

hitInfos数组不确保按照（碰撞点point到射线开始端点ray.origin的）距离排序，如果需要排序，应该使用`Physics.RaycastAll`（但会产生新数组，造成GC），并使用排序函数

```c#
hitInfos = Physics.RaycastAll(ray, 999f);
Array.Sort(hitInfos, (left, right) => { return left.distance.CompareTo(right.distance); });
```

如果使用`Physics.RaycastNonAlloc`再进行排序，需要只对`hitInfosLength`之内的元素进行排序，因为之外的元素是无效的

参考：

- [RaycastNonAlloc sort collision from furthest to closest? - Unity Forum](https://forum.unity.com/threads/raycastnonalloc-sort-collision-from-furthest-to-closest.588013/)
- [Unity - Scripting API: Physics.RaycastNonAlloc (unity3d.com)](https://docs.unity3d.com/ScriptReference/Physics.RaycastNonAlloc.html)