- 应当以打包后实际效果为准
- 如果不是Update中修改`transform.position`移动，而是在FixedUpdate中调用`rigidbody.MovePositon`或修改`transform.position`移动，应当在`Project Settings -> Time -> Fixed Timestep`修改为小于1/60的值，比如`0.0166666`，这个值可能与屏幕支持的刷新率有关

参考：[Smooth movement in Unity | zubspace - between code and design](https://www.zubspace.com/blog/smooth-movement-in-unity)