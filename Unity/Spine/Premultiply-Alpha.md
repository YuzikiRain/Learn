**黑边**

![黑线问题.jpg](http://esotericsoftware.com/forum/download/file.php?id=825)

导出的 PMA，渲染为 Straight，你得到黑边（没有使用bleed）

这是由于使用了非Point的线性Filter，这些filter会使用周围的像素来平滑，而那些alpha为0的像素仍被考虑为RGB为0,0,0的像素，因此得到了黑边



> 内特写道：
>
> 一些背景故事，所以每个人都一定会跟随： 有时图像像素不会 1 到 1 映射到屏幕像素，例如，当图像被缩放或旋转或放置在整数坐标之间时。这基本上一直发生在骨骼动画中。当它发生时，“过滤”用于确定在屏幕上显示什么。“最近”过滤选择最近的像素。这会产生锯齿状的边缘，这对于复古风格的像素艺术来说是可以的。更经常使用“线性”过滤，它平均周围的像素以产生更平滑的图像。
>
> 那么，线性过滤会平均周围像素的颜色，对吗？当一些周围像素的 alpha 为零时会发生什么？他们仍然得到平均！这意味着您看不到的像素的颜色（零 alpha）会影响您可以看到的像素的颜色。许多图像编辑程序对 alpha 为零的像素使用 0,0,0 的 RGB，这会导致在启动线性过滤时图像的边缘被染成黑色。
>
> 一种（主要）在不使用 PMA 的情况下解决此问题的方法就是利用Spine的[出血](http://esotericsoftware.com/forum/viewtopic.php?p=13480#p13480)功能。但是，当使用 PMA 时，问题就完全消失了。
>
> 一些更详细的阅读：
>
> [http](http://blogs.msdn.com/b/shawnhar/archive/2009/11/02/texture-filtering-alpha-cutouts.aspx) : [//blogs.msdn.com/b/shawnhar/archive/2009/11/02/texture-filtering-alpha-cutouts.aspx](http://blogs.msdn.com/b/shawnhar/archive/2009/11/02/texture-filtering-alpha-cutouts.aspx)
>
> http://blogs.msdn.com/b/shawnhar/archive/2009/11/06/premultiplied-alpha.aspx

参考：http://esotericsoftware.com/forum/Premultiply-Alpha-3132

## Spine的纹理压缩和半透显示

[Spine的纹理压缩和半透显示 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/76077345)