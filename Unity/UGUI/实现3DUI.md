## Screen Space - Camera

将Canvas的Render Mode设置为Screen Space - Camera，然后将对应的camera设置为perspective透视投影。

推荐使用该方案，需要有透视效果的UI才修改其旋转或到相机的距离，就能实现3D透视效果，且不会影响其他同Canvas的UI（z为0，在近平面上，也能受CanvasScaler影响而实现分辨率适配）

## World Space

将Canvas的Render Mode设置为World Space，UICamera和所有的UI需要小心地设置位置，因为（UI的localPosition.z为0时）不会自动将UI设置到近平面上