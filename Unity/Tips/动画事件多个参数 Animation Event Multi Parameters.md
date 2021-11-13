### 多个参数

``` csharp
public void MyAnimationEventHandler (AnimationEvent animationEvent)
 {
     string stringParm = animationEvent.stringParameter;
     float floatParam = animationEvent.floatParameter;
     int intParam = animationEvent.intParameter;

     // Etc.
 }
```
  <img alt="Sprite.png" src="assets/Animation Event Multi Parameter.png" width="500" height="" >

### 单个参数

如果直接使用string int float等作为参数而不是AnimationEvent，那么仅支持单个参数
``` csharp
public void PlayEffect(string name)
{
    Debug.Log(name);
}
```
  <img alt="Sprite.png" src="assets/Animation Event Single Parameter.png" width="500" height="" >

### 参考
[Unity](https://answers.unity.com/questions/722410/animation-events-how-is-the-parameter-animationeve.html)
