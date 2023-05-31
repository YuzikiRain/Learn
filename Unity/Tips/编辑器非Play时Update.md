- Mono脚本添加`[ExecuteAlways]`标签，然后在Update函数里写逻辑。
- 在适当时机（比如一开始启动编辑器时）通过`UnityEditor.EditorApplication.update += EditorUpdate;`注册监听。

编辑器下Update时，如果需要在Scene视图下观察到流畅的表现，需要点击Scene工具栏上的显示设置，勾选`Always Refresh`。
