### 

```
public class ParallaxScrollingFarX : MonoBehaviour
{
    private void Update()
    {
        UpdateParallaxScrolling();
    }
}
```

ParallaxScrollingFarX脚本的Update时机与Cinemachine Brain脚本的Update时机不同，需要找出 Cinemachine Brain 脚本的Update时机

### 解决方案

在脚本被激活时，添加对CameraUpdatedEvent事件的监听即可

```csharp
private void OnEnable()
{
    CinemachineCore.CameraUpdatedEvent.AddListener(CameraUpdate);
}

private void OnDisable()
{
    CinemachineCore.CameraUpdatedEvent.RemoveListener(CameraUpdate);
}

private void CameraUpdate(CinemachineBrain arg0)
{
    UpdateParallaxScrolling();
}
```

### 参考

[Unity Cinemachine Forum by Gregoryl](https://forum.unity.com/threads/simple-2d-parallax-with-orthographic-pixel-perfect-cameras.762977/#post-5082272)

