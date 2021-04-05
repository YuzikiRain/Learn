``` csharp
// Reload 会使得 BuildRoot 和 BuildRows 依次被调用
Reload();
// 决定TreeView的根节点，因此需要在这一步进行根节点初始化
protected override TreeViewItem BuildRoot()
// TreeView按IList的顺序依次显示每行元素
protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
// 每行如何绘制GUI，可做一些自定义如显示icon
protected override void RowGUI(RowGUIArgs args)
// 选中项变化时的回调
protected override void SelectionChanged(IList<int> selectedIds)
// 从 searchFromThisItem 开始搜索指定id的TreeViewItem，用rootItem搜索整个TreeView
protected TreeViewItem FindItem(int id, TreeViewItem searchFromThisItem)
// 根物体，隐藏的，不会被渲染出来
rootItem
```

### DragAndDropVisualMode

-   None：显示禁止icon
-   Rejected：显示禁止icon
-   Copy：显示加号icon
-   Link：显示链接icon
-   Move：显示移动icon
-   Generic：等同于Copy

### 坑

-   DragAndDropVisualMode 设置为 None 和 Rejected时，即使松开鼠标左键 DragAndDropArgs.performDrop 也不会为 true