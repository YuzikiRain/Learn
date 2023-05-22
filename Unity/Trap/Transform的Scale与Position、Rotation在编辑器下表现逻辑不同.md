在Inspector里的Position、Rotation、Scale都是表示本地的，而不是世界的。

修改层次结构时（比如设置parent为其他物体），会保持物体的世界位置、旋转、缩放不变的情况下，修改本地对应的Position、Rotation、Scale。

举例：

有C和P两个物体，将P的localScale.xyz都设置为2，C的localScale.xyz都设置为1。

然后将C的parent设置为P，此时发现物体C的Inspector上的localScale.xyz都变为了0.5。

这是因为C之前的世界缩放为1，为了保持世界缩放在设置parent后仍为1，C的localScale则被设置为0.5。