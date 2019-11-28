标签 RuntimeInitializeOnLoadMethod 还具有属性可以设置，与脚本执行顺序比较如下：

**RuntimeMethodLoadBefore** –> Awake –> OnEnable –> **RuntimeMethodLoadAfter** –> **RuntimeMethodLoad** –> Start

### 参考
https://www.bbsmax.com/A/Ae5RL4xmzQ/
https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html
