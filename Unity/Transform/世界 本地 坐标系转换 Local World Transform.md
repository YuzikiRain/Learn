### TransformPoint TransformVector TransformDirection

-   TransformPoint：将本地空间下的某个点变换到世界空间下，受Transform的位置、旋转、缩放影响

    ``` csharp
    using UnityEngine;
    
    public class TransformTest : MonoBehaviour
    {
        [SerializeField] private Transform localTransform;
    
        void Update()
        {
            localTransform.localPosition = new Vector3(11f, 12f, 13f);
            localTransform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            localTransform.localScale = new Vector3(2f, 3f, 4f);
            var delta = new Vector3(5f, 6f, 7f);
    
            // TransformPoint
            Vector3 worldPosition = localTransform.TransformPoint(delta);
            // caculate manually, iterate every parent Transform until we reach root
            Transform currentTransform = localTransform;
            Vector3 worldPositionByCaculate = delta;
            while (currentTransform)
            {
                worldPositionByCaculate = currentTransform.localPosition + currentTransform.localRotation * Multi(worldPositionByCaculate, currentTransform.localScale);
                currentTransform = currentTransform.parent;
            }
    
            Debug.Log($"{nameof(worldPosition)} = {worldPosition} {nameof(worldPositionByCaculate)} = {worldPositionByCaculate}");
        }
    
        private Vector3 Multi(Vector3 vector3, Vector3 scaleFactor)
        {
            return new Vector3(vector3.x * scaleFactor.x, vector3.y * scaleFactor.y, vector3.z * scaleFactor.z);
        }
    }
    ```
    
-   TransformVector：将本地空间下的某个向量变换到世界空间下。仅受 Transform 的旋转和缩放影响，因为向量指具有大小（magnitude）和方向的量，没有位置这个说法。

    ``` csharp
    using UnityEngine;
    
    public class TransformTest : MonoBehaviour
    {
        [SerializeField] private Transform localTransform;
    
        void Update()
        {
            localTransform.localPosition = new Vector3(11f, 12f, 13f);
            localTransform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            localTransform.localScale = new Vector3(2f, 3f, 4f);
            var delta = new Vector3(5f, 6f, 7f);
    
            Vector3 worldVectorByCaculate = delta;
            Transform currentTransform = localTransform;
    
            // TransformVector
            Vector3 worldVector = currentTransform.TransformVector(delta);
            // caculate manually, iterate every parent Transform until we reach root
            while (currentTransform)
            {
                worldVectorByCaculate = currentTransform.localRotation * Multi(worldVectorByCaculate, currentTransform.localScale);
                currentTransform = currentTransform.parent;
            }
    
            Debug.Log($"{nameof(worldVector)} = {worldVector} {nameof(worldVectorByCaculate)} = {worldVectorByCaculate}");
        }
    
        private Vector3 Multi(Vector3 vector3, Vector3 scaleFactor)
        {
            return new Vector3(vector3.x * scaleFactor.x, vector3.y * scaleFactor.y, vector3.z * scaleFactor.z);
        }
    }
    ```
    
-   TransformDirection：将本地空间下的某个向量变换到世界空间下。仅受旋转以及**缩放的正负**影响（因为scale也间接影响了旋转，这是一个坑），因为方向没有大小和位置（**注意！得到的Vector3并不是归一化后的**）

    ``` csharp
    using UnityEngine;
    
    public class TransformTest : MonoBehaviour
    {
        [SerializeField] private Transform localTransform;
    
        void Update()
        {
            localTransform.localPosition = new Vector3(11f, 12f, 13f);
            localTransform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            localTransform.localScale = new Vector3(2f, 3f, 4f);
            var delta = new Vector3(5f, 6f, 7f);
    
            Vector3 worldDirectionByCaculate = delta;
            Transform currentTransform = localTransform;
    
            // TransformDirection
            Vector3 worldDirection = currentTransform.TransformDirection(delta);
            // caculate manually, iterate every parent Transform until we reach root
            while (currentTransform)
            {
                worldDirectionByCaculate = currentTransform.localRotation * MultiSign(worldDirectionByCaculate, currentTransform.localScale);
                currentTransform = currentTransform.parent;
            }
    
            Debug.Log($"{nameof(worldDirection)} = {worldDirection} {nameof(worldDirectionByCaculate)} = {worldDirectionByCaculate}");
        }
    
        private Vector3 MultiSign(Vector3 vector3, Vector3 scaleFactor)
        {
            return new Vector3(vector3.x * Mathf.Sign(scaleFactor.x), vector3.y * Mathf.Sign(scaleFactor.y), vector3.z * Mathf.Sign(scaleFactor.z));
        }
    }
    ```
    

假设父子关系为 root -> parent -> local，root为根物体，变换仅包含缩放变换。从root坐标系到到parent坐标系的变换矩阵为M1，从parent坐标系到到local坐标系的变换矩阵为M2，在root坐标系下表示的一个矢量$V_a$，变换到local坐标系下为矢量$V_b$，已知$V_b$，求$V_a$

显然有等式 $M_2 (M_1 V_a) = V_b$ 和 $V_a = M_1^{-1} M_2^{-1} V_b$

而这里的 $M_2^{-1}$其实就是 local相对于parent的localScale，$M_1^{-1}$则是parent相对于root的localScale

比如：有某个子空间和父空间，子空间.localScale = (0.5, 0.5, 0.5)，表示子空间的每1单位都只有父空间的0.5单位，反过来说就是，父空间的1单位就代表子空间的2单位，即坐标轴单位缩放了0.5倍，向量从父空间变换到子空间则是缩放了2倍。在父空间下的矢量(1,1,1)，变换到子空间下则是(2,2,2)而不是(0.5,0.5,0.5)

localScale 的含义就是**相比于父空间坐标单位，本地坐标单位被缩放了scale倍，从父空间到子空间的向量缩放了 1/scale 倍**

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

## ScreenToWorldPoint

`public Vector3 ScreenToWorldPoint(Vector3 screenPosition);`

输入参数screenPosition的xy分量为屏幕坐标（一般就是Input.mousePosition），z分量还要再加上需要的深度（一般为相机的位置的z分量加上近平面）

如果z分量没有加上需要的深度，当然就是默认的0了，这样一来相当于转换到（透视）相机的近平面为0的平面上，screenPosition变化量再大，返回的世界坐标的变化量也为0

## 参考

-   https://irfanbaysal.medium.com/differences-between-transformvector-transformpoint-and-transformdirection-2df6f3ebbe11