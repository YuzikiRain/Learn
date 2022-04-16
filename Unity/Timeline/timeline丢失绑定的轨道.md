## 代码剥离问题

## TimelineAsset被Prefab错误引用（且产生了错误的binding）

有时可以找到绑定，有时不能。部分可以，部分不能，那么不是代码剥离问题（否则就是全都丢失绑定或者都不丢失）

![image-20220416230938671](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220416230938671.png)

### 解决方案

- Timeline插件升级到1.6.4或更新版本，打开每一个有PlayableDirector组件的prefab，点击减号删除错误的binding（最好删除所有的）
    也可以参考减号对应的函数调用，写自定义的代码处理所有涉及的prefab
- 清除错误的（最好是全部）PlayableDirector组件的m_SceneBindings序列化字段

[如何编写代码以删除 PlayableDirector 中未使用的绑定？- 统一答案 (unity.com)](https://answers.unity.com/questions/1833426/how-can-i-code-to-delete-unused-bindings-in-playab.html)