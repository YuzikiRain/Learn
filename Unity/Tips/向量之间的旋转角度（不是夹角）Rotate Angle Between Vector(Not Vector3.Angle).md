### 用途
已知角色的面朝方向和目标位置，求角色要朝哪个方向旋转多少角度才能正好面朝目标

### 代码

``` csharp
using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.SignedAngle(forward, targetDir, Vector3.up);
        if (angle < -5.0F)
            print("turn left");
        else if (angle > 5.0F)
            print("turn right");
        else
            print("forward");
    }
}
```
将角色绕Vector3.up顺时针旋转angle度即可面朝target

- float angle = Vector3.SignedAngle(from, to, axis)
- 向量forward与targetDir的叉积（左手/右手坐标系）得到向量up
- 向量up与axis点积的结果>0，那么绕向量up（左手/右手坐标系）旋转angle度即可；点积的结果 < 0，那么绕向量up（左手/右手坐标系）旋转-angle度即可


### 参考
https://docs.unity3d.com/ScriptReference/Vector3.SignedAngle.html
