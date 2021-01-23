### Rect Transform
假设Canvas Scaler的Screen Match Mode为Match With Screen Size，Match为Width

Match为Width，表示**保持Rect Transform的Width为Reference Solution.width，缩放Scale使得实际width等于屏幕width**
##### Width Height

由于Match为Width，那么
- Width = Reference Solution.width
- Height = Reference Solution.height / Scale
##### Scale
- Screen Space - Overlay：
由于Match为Width，缩放**Scale = 屏幕分辨率.width / Reference Resolution.width** 假设Canvas Scaler的Reference Solution设置为1920*1080，对于2560*1080的屏幕分辨率，Scale = 2560 / 1920 = 1.33333333，Scaler脚本将整个Canvas放大了Scale倍，这样“看起来”整个Canvas就像是2560像素的宽度了。
- Screen Space - Camera：
     ``` csharp
     // 因为Match Width，所以Canvas的宽度自然等于Reference Resolution的宽度
    Canvas.width = Reference.width
    // 和Screen Space - Overlay一样，需要缩放ScreenScaleToCanvas倍才能使Canvas.width的像素尺寸和屏幕宽度一致。
    ScreenScaleToCanvas = 屏幕分辨率.width / Canvas.width
    // Canvas.width也要缩放这么多，求得Canvas.width
    Canvas.height = 屏幕分辨率.height /ScreenScaleToCanvas
    // 用Render Camera的（Veritical）Size * 2得到实际相机渲染尺寸，求得要将Canvas缩放Scale倍才能刚好使Canvas和相机尺寸一致
    Canvas.Scale = Render Camera.Size * 2 / Canvas.height
    ```

##### Position
- Screen Space - Overlay：Position就是屏幕分辨率/2（这样使得**Canvas的左下角刚好在世界空间原点**），z永远为0
- Screen Space - Camera：Position就是Canvas组件中**Render Camera对应的Camera的位置**，z = 相机.position.z + Canvas.Plane Distance

### Canvas Scaler
##### UI Scale Mode
- Constant Pixel Size：
固定像素大小，因此Canvas的Scale就是Scale Factor，Canvas的尺寸就是屏幕分辨率/Scale
- Scale Width Screen Size：
    根据Reference Resolution和Screen Match Mode对Canvas进行缩放使得Canvas尺寸 * Scale = 屏幕分辨率。
    Screen Match Mode
    - Width：
    表示**保持Rect Transform的Width为Reference Solution.width，缩放Scale使得实际width等于屏幕width**。Height同理。
    - Expand：假设Reference Solution为1920*1080，屏幕分辨率为2560*1280，那么将会先缩放1280/1080倍使得Canvas.height匹配屏幕分辨率.height，然后将Canvas.width扩大使得宽高比仍是2560/1280
    - Shrink：假设Reference Solution为1920*1080，屏幕分辨率为2560*1280，那么将会先缩放2560/1920倍使得Canvas.width匹配屏幕分辨率.width，然后将Canvas.height缩减使得宽高比仍是2560/1280
##### Reference Pixels Per Unit
默认值100，即每100像素占据1单位

### Canvas

##### Render Mode

- Screen Space - Overlay：
默认将所有UI元素绘制在屏幕之前，不论其他相机如何设置。
因此只有将部分UI设置成透明的，才能够在屏幕上显示出其他相机所渲染的非UI对象。（从shader的角度来看，不透明UI会和其他非UI对象或者说颜色缓冲区中的颜色进行透明度混合）
如果有多个Canvas，其Sort Order越大，显示在越前边（从shader的角度来看，Sort Order越大则渲染顺序越靠后，会将之前的颜色缓冲区覆盖或混合）
- Screen Space - Camera：
只要设置好Render Camera，就跟多个相机之间的渲染类似了。如果未设置那么效果跟Overlay一样
- World Space：
完全把Canvas当作世界坐标中的一个物体来渲染，此时将Canvas视作一个矩形的Mesh即可。

##### Override Sorting

有时候会遇到想要将一些非UGUI元素如Particle、Sprite、3D模型等显示在两个UI之间，而非Screen Space - Overlay和Screen Space - Camera模式下的UI由于用额外的相机来渲染，要么全部在非UGUI元素之前要么在之后

-   给指定UGUI元素和非UGUI元素附加Canvas组件，勾选Override Sorting
-   设置合适的Sorting Layer，使得非UGUI元素显示正确

勾选了Override Sorting的UGUI元素使用SortingLayer来排序，而不再由在Canvas中的层次顺序决定，因此与其他默认UGUI元素之前渲染顺序无法得到保证。
但其**子物体中的UGUI元素之间**的渲染顺序仍由层次顺序决定

### GraphicsRayCaster

Canvas组件所在物体或子物体拥有GraphicsRayCaster组件，GraphicsRayCaster组件的子物体才能收到对应UI事件