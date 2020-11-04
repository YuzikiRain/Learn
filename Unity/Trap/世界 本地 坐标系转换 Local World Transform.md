### transform.localPosition

``` csharp
// 将表示方向的向量从世界坐标系转换到本地坐标系，忽略缩放影响
Vector3 localPosition = target.parent.InverseTransformDirection(offset);
localPosition == target.localPosition;
// 然而target的世界坐标并不等同于父物体的世界坐标加上自身的本地坐标
// 因为target的父物体很可能被旋转过，使得父物体的本地坐标系不等于世界坐标系
target.position != target.parent.position + target.localPosition;
// 实际上，localPosition表示的是在父物体的本地坐标系下的偏移量
target.position == target.parent.position + target.parent.rotation * target.localPosition;
```

### Vector3从World到Local

```csharp
Transform target;
// 世界坐标系下的(1, 0, 0)
Vector3 offset = Vector3.right;

// 以下变量都表示（在世界坐标系下初始化表示的）offset在target的本地坐标系下的值
var b = target.parent.InverseTransformDirection(offset);
var d = Quaternion.Inverse(target.parent.rotation) * offset;
var e = b.normalized * offset.magnitude;
b === d === e
```

