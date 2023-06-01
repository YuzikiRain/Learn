- Mono脚本添加`[ExecuteAlways]`标签，然后在Update函数里写逻辑。
- 在适当时机（比如一开始启动编辑器时）通过`UnityEditor.EditorApplication.update += EditorUpdate;`注册监听。

# 表现不流畅

- 在Scene视图下：需要点击Scene工具栏上的显示设置，勾选`Always Refresh`。
- 在Game视图下：可能需要鼠标在Game视图上不断移动强制其刷新。
