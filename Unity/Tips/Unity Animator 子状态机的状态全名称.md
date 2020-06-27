```
public void CrossFadeInFixedTime(string stateName, float fixedTransitionDuration);
```
stateName由三部分组成，`LayerName.SubStateName.StateName`

**等同于以下代码**

```
int stateHashName = Animator.StringToHash("LayerName.SubStateName.StateName")
CrossFadeInFixedTime(int stateHashName, fixedTransitionDuration)
```

CrossFadeInFixedTime函数的另一个重载
```
public void CrossFadeInFixedTime(int stateHashName, float fixedTransitionDuration);
```

参考自 https://docs.unity3d.com/ScriptReference/Animator.CrossFadeInFixedTime.html





