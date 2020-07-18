标签 RuntimeInitializeOnLoadMethod 还具有属性可以设置，与脚本执行顺序比较如下：

**SubsystemRegistration** –> **AfterAssembliesLoaded** -> **BeforeSplashScreen** -> **BeforeSceneLoad** -> Awake –> OnEnable –> **AfterSceneLoad** –> Start -> Update

### 参考
https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html
