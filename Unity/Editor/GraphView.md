### GraphView：

一张图，应该使用EditorWindow来创建它的单个实例，一般来说一个EditorWindow里应该只有一个GraphView

### Node：

节点

-   title：Node的标题名称
-   inputContainer outputContainer ：装着 Port 的容器

### Port

### 端口，分为入端口和出端口

-   portName：端口显示名称

### Edge：端口之间的连线





常用API

```csharp
// 为Node创建Port
Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
// 为Node添加自定义Element后，调用这个函数进行刷新，否则这些新Element不会被看到
node.RefreshExpandedState();
// 刷新Port的布局
node.RefreshPorts();
// 设置Node的位置，一般用于在新建Node之后的初始位置
node.SetPosition(new Rect())
```

