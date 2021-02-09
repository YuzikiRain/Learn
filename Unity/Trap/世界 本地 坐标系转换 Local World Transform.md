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

### TransformPoint TransformDirection TransformVector

-   Point：将本地空间下的某个点变换到世界空间下，受Transform的位置、旋转、缩放影响

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
    

-   Vector：将本地空间下的某个向量变换到世界空间下。仅受 Transform 的旋转和缩放影响，因为向量指具有大小（magnitude）和方向的量，没有位置这个说法。

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
-   Direction：将本地空间下的某个向量变换到世界空间下。仅受旋转影响**以及所有父级的 scale 影响（因为scale也间接影响了旋转，这是一个坑）**，因为方向没有大小和位置（**注意！得到的Vector3并不是归一化后的**）

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
                worldDirectionByCaculate = currentTransform.localRotation * worldDirectionByCaculate;
                currentTransform = currentTransform.parent;
            }
    
            Debug.Log($"{nameof(worldDirection)} = {worldDirection} {nameof(worldDirectionByCaculate)} = {worldDirectionByCaculate}");
        }
    }
    ```
    

### 参考

-   https://irfanbaysal.medium.com/differences-between-transformvector-transformpoint-and-transformdirection-2df6f3ebbe11