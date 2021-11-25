### 位置

添加RectTransform组件（而不是用默认的Transform组件），然后设置anchoredPosition或anchoredPosition3D为UI层上的位置（transform的坐标系和RectTransform的不同）

### 显示顺序

有时候会遇到想要将一些非UGUI元素如Particle、Sprite、3D模型等显示在两个UI之间，非Screen Space - Overlay和Screen Space - Camera模式下的UI由于用额外的相机来渲染，要么全部在非UGUI元素之前要么在之后。想要UI和其他物体穿插显示，必须指定其与UI之间的绘制顺序（默认UGUI的绘制顺序是层级正序），可通过以下方式：

-   给指定UGUI元素和非UGUI元素附加Canvas组件，勾选Override Sorting
-   设置合适的Sorting Layer，使得非UGUI元素显示正确

勾选了Override Sorting的UGUI元素使用SortingLayer来排序，而不再由在Canvas中的层次顺序决定，因此与其他默认UGUI元素之前渲染顺序无法得到保证。
但其**子物体中的UGUI元素之间**的渲染顺序仍由层次顺序决定
