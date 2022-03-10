SpriteAtlas默认是勾选`allow rotation`和`tight packing`的，这会为Sprite（根据透明度边缘）生成多边形的mesh，而新生成的Image组件在使用Image Type为Simple时，默认是不勾选`Use Sprite Mesh`的，**应该勾选此项以应用生成的紧密mesh**

### 优点

- 紧密图集减小的包体大小和内存占用
- 减少透明度为0的区域造成的overdraw

### 缺点

- 生成的mesh的顶点一般较多，如果屏幕上的sprite过多，可能较多的顶点也会造成瓶颈

是否勾选`Use Sprite Mesh`需要对项目进行实际测试

### 参考

[Unity Sprites: Full-Rect or Tight? | TheGamedev.Guru](https://thegamedev.guru/unity-gpu-performance/sprites-full-rect-or-tight/#:~:text=You can set the mesh type as tight,so you minimize the transparent area you render.)

Use Sprite Mesh选项在2018.3版本开始可用

[Unity - Scripting API： UI.Image.useSpriteMesh (unity3d.com)](https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Image-useSpriteMesh.html)