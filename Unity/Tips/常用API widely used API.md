### 3D

#### 将世界空间方向转换为本地空间方向

范例：角色transform.forward = (1, 0, 0)（世界空间），Velocity = (0, 0, 1)（世界空间），通过以下函数

```csharp
var direction = transform.InverseTransformDirection(Velocity);
```

得出direction为(-1, 0, 0)，即角色正在朝**角色左方向**移动

#### （同一空间下）将矢量(0, 0, 1)旋转到**其正上方为upwards，正前方为forward**所需要的旋转矩阵

```csharp
public static Quaternion LookRotation(Vector3 forward, Vector3 upwards = Vector3.up);
```

#### （同一空间下）将fromDirection旋转到toDirection所需要的旋转矩阵

```csharp
public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection);
```

