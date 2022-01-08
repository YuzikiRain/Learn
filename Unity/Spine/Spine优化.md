#### [Changing Materials Per Instance](http://zh.esotericsoftware.com/spine-unity#Changing-Materials-Per-Instance)

##### **Tinting Skeletons while retaining batching**

在skeleton实例上使用不同的 `Materials` 或 `MaterialProeprtyBlocks` 会破坏batching. 如果你只需要tint某个skeleton实例且不修改其他material属性, 可以使用 `Skeleton.R .G .B .A` 颜色属性. 为了使tinting生效, 必须在SkeletonRenderer检查器中启用[`Advanced - PMA Vertex Colors`](http://zh.esotericsoftware.com/spine-unity#Setting-Advanced-Parameters).

``` csharp
public Color color = Color.white;
...
skeleton = GetComponent<SkeletonRenderer>().Skeleton;
...
skeleton.R = color.r;
skeleton.G = color.g;
skeleton.B = color.b;
skeleton.A = color.a;
```

这些skeleton颜色值将置为顶点颜色且不改变material属性.

这也同样适用于tinting某个附件:

``` csharp
slot = skeleton.FindSlot(slotname);
...
slot.R = slotColor.r;
slot.G = slotColor.g;
slot.B = slotColor.b;
slot.A = slotColor.a;
```

> **注意:** 当你的动画修改附件的颜色值时, 一定要使用诸如 [SkeletonAnimation.UpdateComplete](http://zh.esotericsoftware.com/spine-unity#Life-cycle) 的回调来在动画应用后设置全部槽位的颜色值.

##### **MaterialPropertyBlocks**

![img](http://zh.esotericsoftware.com/img/spine-runtimes-guide/spine-unity/materialpropertyblock-demo.gif)

可以使用 [Renderer.SetPropertyBlock](http://docs.unity3d.com/ScriptReference/Renderer.SetPropertyBlock.html) 来覆盖单个 `MeshRenderer` 的material属性值.

``` csharp
MaterialPropertyBlock mpb = new MaterialPropertyBlock();
mpb.SetColor("_FillColor", Color.red); // "_FillColor" is a named property on the used shader.
mpb.SetFloat("_FillPhase", 1.0f); // "_FillPhase" is another named property on the used shader.
GetComponent<MeshRenderer>().SetPropertyBlock(mpb);

// to deactivate the override again:
MaterialPropertyBlock mpb = this.cachedMaterialPropertyBlock; // assuming you had cached the MaterialPropertyBlock
mpb.Clear();
GetComponent<Renderer>().SetPropertyBlock(mpb);
```

> **注意:** 在 `MaterialPropertyBlock` 中使用的参数名, 如 `_FillColor` 或 `_FillPhase`, 须与对应的着色器变量名匹配. 请注意, 着色器变量名不是显示在检查器中的名称, 例如 `Fill Color` 和 `Fill Phase`. 要查看着色器的参数名, 你可以打开 `.shader` 文件(通过点击material的齿轮图标, 选择 `Edit Shader`), 查看最前面的 `Properties { .. }` 这段代码, 那里有全部参数的列表. 在如下这行参数中, **最左边的**名字就是参数名 `_FillColor`:
>
> `_FillColor ("Fill Color", Color) = (1,1,1,1)`
>
> 着色器变量名通常以 `_` 字符开头, 且从不包含任何空格. 它旁边的字符串 `"Fill Color"` 就是在检查器中显示的名称.

你可以在示例场景 `Spine Examples/Other Examples/Per Instance Material Properties` 中找到对每个实例material属性的演示.

> **优化提示**
>
> - 使用具有不同Material值的Renderer. SetPropertyBlock会破坏渲染器之间的batching. 当MaterialPropertyBlock的参数相等时(例如所有的tint颜色都设置为绿色)那么渲染器间将进行batching.
> - 每当你改变或增加MaterialPropertyBlock的属性值时, 你都需要调用 `SetPropertyBlock`. 但你可以把MaterialPropertyBlock作为你的类的成员, 这样你就不必在改变某个属性值时不得不实例化出一个新的.
> - 当你需要经常设置某个属性时, 你可以使用静态方法: `Shader.PropertyToID(string)` 来缓存该属性的int ID, 而不是字符串重载MaterialPropertyBlock的setter.

#### [Material 切换(Switching)和绘制调用](http://zh.esotericsoftware.com/spine-unity#Material-切换(Switching)和绘制调用)

如果附件分布在多个atlas页上, 比如material `A`和material `B`, 运行时会根据需要material的绘制顺序来设置material数组.

若需求顺序为:

> 1. A的附件
> 2. A的附件
> 3. B的附件
> 4. A的附件

则生成的material数组为:

> 1. Material A (满足需求1和2)
> 2. Material B (满足需求3)
> 3. Material A (满足需求4)

Material数组中的每一个material都对应着一次[绘制调用](http://docs.unity3d.com/Manual/DrawCallBatching.html). 因此大量的material切换会削弱性能表现.

Dragon示例展示了这种有很多绘制调用的用例:

![img](http://zh.esotericsoftware.com/img/spine-runtimes-guide/spine-unity/render_spineunity_alternatingmaterials.png)

因此, 建议将附件打包到尽可能少的atlas页中, 并根据绘制顺序将附件分组置入atlas页以防止多余的material切换. 请参阅[Spine Texture Packer: Folder Structure](http://esotericsoftware.com/spine-texture-packer#Folder-structure)了解如何在你的Spine atlas中编排atlas区域.