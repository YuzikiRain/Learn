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

## 再说一句

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

