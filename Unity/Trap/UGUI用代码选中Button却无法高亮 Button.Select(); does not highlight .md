## 现象
在继承MonoBehaviour的脚本的OnEnable或Awake函数中使用button.Select()函数，且附加该脚本的UI物体是被动态Instantiate出来的，当UI物体被创建出来时，被选定的按钮不会高亮，而使用方向键移动后才会显示按钮的高亮
```csharp
using UnityEngine;
 using UnityEngine.UI;

 public class ButtonSelector : MonoBehaviour {

     public Button selectButton;

     void OnEnable()
     {
         if(selectButton != null)
         {
             selectButton.Select();    
             print("selected button");
         }
         else
             Debug.Log ("SelectButton was null");
     }
 }
```

## 解决方法
- 将button.Select() 或 EventSystem.SetSelectedGameObject(button.gameObject) 方法放到Start()方法中，而不是Awake或Enable方法中

### 参考：
- [Button.Select(); does not highlight? Unity Question](https://answers.unity.com/questions/1142958/buttonselect-doesnt-highlight.html)
