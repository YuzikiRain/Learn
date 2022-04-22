用固定顺序的欧拉角来描述旋转时，如果第二次旋转的角度为90或-90度，则会产生万向节死锁

## 重现万向节死锁

### 步骤

- Unity采用左手坐标系，Unity中的（Transform组件的以欧拉角表示的）旋转顺序为：**z-x-y**
- 物体的默认朝向为世界空间的z轴，即(0,0,1f)
    ![image-20220422105249074](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220422105249074.png)
- 绕物体自身z轴旋转30度，**物体以世界空间为参考系绕z轴旋转了30度**
    ![image-20220422105402343](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220422105402343.png)
- 绕物体自身x轴旋转-90度，发生了旋转
    ![image-20220422111503750](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220422111503750.png)
- 绕物体自身y轴旋转30度，**物体以世界空间为参考系绕z轴旋转了30度**
    ![image-20220422111851736](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220422111851736.png)

### 解读

第二次绕x轴旋转90度后，第一次的绕绕物体自身z轴旋转和第三次的绕物体自身y轴旋转**都是相对于以世界空间为参考系绕z轴旋转**

此时（只要保持欧拉角x为-90不变）**将欧拉角的y和z改为其他任何数值，物体都只能进行yaw，而不会（相对自身）进行roll了**，这就发生了万向节死锁

第二次的绕x轴旋转-90度（或90度）使得第一次绕z轴旋转产生的（以自身为参考系）**roll**效果变为了（以自身为参考系的）**yaw**效果

## Unity的Rotate和Transform.eulerAngles

unity的inspector面板中的欧拉角只是编辑器扩展的显示，实际是用四元数存储的，最后反推出欧拉角并显示

![image-20220422113309814](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220422113309814.png)

如果像上一节中的方式操作，第三步操作中修改inspector的rotate的x分量为90时，会发现z分量和y分量也被修改了，z变为30，而y变为了-30

### 两者的关系

- inspector里设置`Transform.localEulerAngles`等同于`Transform.Rotate(eulerAngles, Space.Self)`

- inspector里设置`Transform.eulerAngles`等同于`Transform.Rotate(eulerAngles, Space.World)`

``` csharp
transform.localEulerAngles = (10f, 20f, 30f);
```

等同于以下，即设置transform.localEulerAngles相当于以当前（即未旋转前）的本地坐标系为参考，先后分别绕z-x-y轴旋转了30、10、20度

``` csharp
transform.Rotate(new Vector3(10f, 20f, 30f), Space.Self);
```

（虽然是按z-x-y的顺序使用欧拉角进行旋转）但不等同于以下，因为每次旋转操作都会改变当前坐标系，即下一次旋转时参考坐标系变了，而之前的操作都是相对于旋转前的本地坐标系为参考，称为**静态欧拉角**

``` csharp
transform.Rotate(new Vector3(0f, 0f, 30f), Space.Self);
transform.Rotate(new Vector3(10f, 0f, 0f), Space.Self);
transform.Rotate(new Vector3(0f, 20f, 0f), Space.Self);
```

`transform.eulerAngles`与`transform.Rotate(eulerAngles, Space.World)`也类似前两者之间的关系。不同的是，旋转前参考的不是本地坐标系，而是世界坐标系

``` csharp
public void Rotate(Vector3 eulerAngles, Space relativeTo = Space.Self);
```

参考：

- [Unity - Scripting API： Transform.Rotate (unity3d.com)](https://docs.unity3d.com/ScriptReference/Transform.Rotate.html)
- [Unity - Scripting API: Transform.eulerAngles (unity3d.com)](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html)

## 静态/动态欧拉角

将物体放置在scene的root下（即没有任何父物体），如果分别设置inspector的rotate的x-y-z为90、30、30，无法得到和第一节相同的结果，看起来物体像是绕着世界坐标系的各个轴旋转的。

造成以上结果的原因是，Unity中的这些操作都是相对于旋转前的坐标轴，称为**静态欧拉角**。第一节中的操作是绕自身坐标轴旋转，而自身坐标轴是会因为旋转而改变的，称为**动态欧拉角**。

具体来说就是：

- 因为没有任何父物体，物体的本地坐标系就是世界坐标系

- **修改inspector的rotate就是修改Transform.localEulerAngles**，但这里的localEulerAngles已经等同于eulerAngles（相对于世界坐标系）
- 设置`transform.eulerAngles`，相当于以当前（即未旋转前）的本地坐标系为参考，先后分别绕z-x-y轴旋转

参考：[动态与静态欧拉角视角下的万向节死锁（Gimbal Lock）问题 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/474447990)

## 参考

- [bonus_gimbal_lock.pdf (krasjet.github.io)](https://krasjet.github.io/quaternion/bonus_gimbal_lock.pdf)
- [【Unity编程】Unity中的欧拉旋转_AndrewFan的博客-CSDN博客_欧拉旋转](https://blog.csdn.net/andrewfan/article/details/60866636)
- [Unity WebGL Player | Objects (andrewfanchina.github.io)](https://andrewfanchina.github.io/UnityLabs/Euler/)