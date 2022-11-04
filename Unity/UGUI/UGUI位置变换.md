## ScreenPoint

```c#
Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
```

screenPosition的**xy分量**表示**屏幕空间坐标**，屏幕空间以**像素为单位**，左下角是(0,0)，右上角是([pixelWidth](https://docs.unity3d.com/ScriptReference/Camera-pixelWidth.html),[pixelHeight](https://docs.unity3d.com/ScriptReference/Camera-pixelHeight.html))

z分量表示在**mainCamera本地空间下的z分量**，单位等同于**默认的空间单位**

### Canvas.planeDistance

一般来说，对于默认相机和物体，会将物体投影到相机的near plane上

但UGUI对UI的处理不同：如果Render Mode为Screen Space - Camera，实际上会

- **将UI元素投影到**对应的UICamrea的`UICamera.transform.position + UICamera.transform.forward * PlaneDistance`的位置（跟UICamera的near plane无关），这个位置正是**对应Canvas面板所在的区域**。
- CanvasScaler根据设置进行Canvas面板的缩放。

### UI跟随

`ScreenToWorldPoint`要求传入UI物体所在位置，其z分量需要由使用者指定，z分量表示在**mainCamera本地空间下的z分量**，单位等同于**默认的空间单位**。

UICamera是Orthographic的时候，z分量是无所谓的，（因为近平面和远平面是一样大）不会受透视除法影响。

如果UICamera是Perspective的，那么进行UI跟随（世界空间中的）3D物体的实现时，需要注意转换时的z位置

``` c#
var screenPosition = mainCamera.WorldToScreenPoint(target.position);
screenPosition.z = canvas.planeDistance;
Vector3 position = uiCamera.ScreenToWorldPoint(screenPosition);
```

一般来说需要设置`screenPosition.z = canvas.planeDistance`，这样不论UICamra的fov是多少都可以忽略透视带来的影响。

如果不这么做，当UI物体不是刚好在canvas面板上（z等于Canvas.planeDistance），则会（先）因透视除法而产生一定的近大远小（然后再进行CanvasScaler）。

比如`Canvas.planeDistance`为100，计算得到的`screenPosition.z`为10（相比canvas更靠近UICamera），则该UI物体会先因为透视除法而放大`100/10=10`倍（然后再进行CanvasScaler，但是这个不影响物体相对于Canvas的大小）

参考：[Unity - Scripting API: Camera.WorldToScreenPoint (unity3d.com)](https://docs.unity3d.com/ScriptReference/Camera.WorldToScreenPoint.html)

最终`ScreenToWorldPoint`将屏幕坐标`screenPosition`变换为世界坐标`worldPosition`：

- 将屏幕坐标变换为相对于Canvas的坐标`canvasPosition`，单位为默认单位而不再是像素。
- 将相对于Canvas的坐标变换为相对于UICamrea的观察坐标，再变换为相对于世界空间的世界坐标：`worldPosition = canvasPosition + UICamera.transform.position + UICamera.transform.forward * PlaneDistance`

### ScreenPointToLocalPointInRectangle 屏幕坐标转换到Rect本地坐标


```
public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint);
```

 - rect: 对应的 RectTransform 的引用
 - screenPoint: 位置，基于屏幕坐标系
 - cam: 相机的引用，如果Canvas的Render Mode 为 ```Screen Space - Overlay``` 模式，则可以只传入null，如果是 ```Screen Space - Camera``` 则需要填入 Render Camera 对应的引用
 - localPoint: **rect 本地坐标系下的坐标（原点(0,0)位置受pivot的影响）**

参考：
https://docs.unity3d.com/ScriptReference/RectTransformUtility.ScreenPointToLocalPointInRectangle.html

```
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
 
public class GlobalTest : MonoBehaviour
{
    public Canvas canvas;
    Text uiText;
    RectTransform referenceRectTransform;
    RectTransform targetRectTransform;
    void Start()
    {
        uiText = canvas.GetComponentInChildren<Text>();
        referenceRectTransform = canvas.GetComponent<RectTransform>();
        targetRectTransform = uiText.GetComponent<RectTransform>();
 
        Debug.Log(targetRectTransform.anchoredPosition);
    }
 
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 outVec;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(referenceRectTransform,Input.mousePosition,null,out outVec))
            {
                Debug.Log("Setting anchored positiont to: " + outVec);
                targetRectTransform.anchoredPosition = outVec;
            }
        }
 
    }
}
```
### 是否在Rect之中

```csharp
bool isInRect = UnityEngine.RectTransformUtility.RectangleContainsScreenPoint(referenceRectTransform, UnityEngine.Input.mousePosition, uiCamera)
```

参考：
https://forum.unity.com/threads/issues-with-recttransformutility-screenpointtolocalpointinrectangle.437246/

### 再说一句

也可以从UGUI的EventSystem中获得鼠标基于屏幕坐标系的点击位置

```
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + "Game Object Click in Progress");
    }
```
参考：
https://docs.unity3d.com/ScriptReference/EventSystems.IPointerDownHandler.html

