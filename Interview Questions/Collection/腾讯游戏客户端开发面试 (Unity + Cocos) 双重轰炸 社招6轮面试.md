## 基础

### 堆和栈

### 补码

### 系统/游戏内存碎片的处理

### windows linux 系统的大小端

### 由a~z组成的字符串，设计压缩算法，哈夫曼编码

### 一道关于排行榜的数据结构体

### 一道2个数组做加法的算法题

### 堆的操作的时间复杂度

### 一亿人排行榜，取第K大

## c++

### 虚函数实现，多态实现，虚函数纯虚函数作用

关键字`virtual`修饰函数即可，多态则是父类指针指向子类对象，调用虚函数时实际会调用子类的被override的虚函数

纯虚函数的类也称为抽象类，是为了让使用者必须override整个类，无法被实例化，从编译器层面禁止了无实际意义的class对象的创建

### STL vector，allocator，map，unorder_map，红黑树 原理  扩展因子 扩容实现

### 如何申请的指针在堆上的地址是64的倍数

### c++单例模式，需要线程安全，并且不能每次都加锁

### 针对线程安全的，锁有多少种

### 自己设计vector怎么选型，用new还是malloc，拷贝用什么？插入n次的时间复杂度是多少？

## 架构

### 核心战斗系统实现 好的设计模式或者架构

ECS BUFF系统

### 大地形管理，开放世界地图策略

大地图：AOI(十字链表法)，LOD，四叉树、八叉树

大地图渲染：LightMap，静态动态和批

### 暗黑三的动态地图生成怎么实现的？

## Unity

###  Unity内存、场景、资源管理，热更新策略

[Learn/AssetBundle.md at master · YuzikiRain/Learn (github.com)](https://github.com/YuzikiRain/Learn/blob/master/Unity/资源和内存管理 asset and memory management/AssetBundle.md)

### 协程和线程区别

[Learn/协程 Coroutine.md at master · YuzikiRain/Learn (github.com)](https://github.com/YuzikiRain/Learn/blob/master/Unity/Script/协程 Coroutine.md)

### 图集策略，打包策略，java和Oc

- 图集：为了最少的内存占用，将常用的且较小的图片比如道具Icon等打成一个图集，如果一个放不下就再按用途分不同图集或者直接按序号递增。由于单个界面的背景图片等都很大，这些相关图片都按界面来分图集。

- 打包：

    - 按实际业务功能需求划分，而不是资源类型。好处是易于管理和迭代热更新，内存占用最小（因为无论如何都要加载），内存管理方便。
        比如一个用spine做的角色，prefab里还需要关联timeline资产，spine资源，攻击时要播放音效。这些资产直接放到一个包里即可。
        如果按类型划分，经常会出现包里的资源没有全部释放，而无法释放包的内存的问题。除非使用选择加载后就立即卸载资源所在的包（`AssetBundle.Unload(false)`）的策略。为了加载一个资源要加载一个ab包，但是由于之前调用`AssetBundle.Unload(false)`已经断开和ab包的链接，为避免再次加载同一个资源，每次加载资源后还需要维护资源列表

    - UI的prefab和其他相关资源都直接打到和图集同一个包即可

### AssetBundle自己封装的嘛？为什么不用现成的？

## C#

### 值类型引用类型，new值类型在栈上还是堆上

分配

``` c#
class SomeClass
{
    public SomeStruct s;
}

class SomeStruct
{
    public int x;
}

static void Main()
{
    int i1 = 0;
    i1 = 1;
    int i2 = new int();
    i2 = 2;
    int i3 = new int();
    i3 = i2;
    i3 = 3;
    // 此时，i1 = 1，i2 = 2，i3 = 3
    
    SomeClass c = new SOmeClass();
    // c.s分配在堆上
    c.s = new SomeStruct();
    
    
}
```

值类型

``` c#
using System;
using System.Collections.Generic;
 
struct Vector2
{
    public int x;
    public int y;
}
 
public class Test
{
	public static void Main()
	{
		List<Vector2> vectors = new List<Vector2>();
		vectors.Add(new Vector2());
		// 不能修改一个中间变量
		//vectors[0].x = 1;
		//Console.WriteLine(vectors[0].x);
		
		var tempVector2 = vectors[0];
		tempVector2.y = 2;
		Console.WriteLine(vectors[0].y);
		
		vectors[0] = new Vector2() {x = 11, y = 22};
		Console.WriteLine(vectors[0].y);
	}
}
```

### GC

[Learn/垃圾回收 Garbage Collect.md at master · YuzikiRain/Learn (github.com)](https://github.com/YuzikiRain/Learn/blob/master/C%23/垃圾回收 Garbage Collect.md)

### C# List对应C++什么，C++ list对应C#什么，STL源码拷问

c# list -> c++ vector

c++ list -> c# LinkedList

## 优化

CPU：

GPU：合批

帧数过高：电池策略，长时间不操作降低亮度和帧率上限

## 渲染

### 光照计算中为什么需要模型空间法线乘以M矩阵的逆转置得到世界空间法线，什么情况下不需要？

[Unity-Shader-Note/渲染流水线和空间变换.md at master · YuzikiRain/Unity-Shader-Note (github.com)](https://github.com/YuzikiRain/Unity-Shader-Note/blob/master/渲染流水线和空间变换.md#法线变换矩阵)

### 背面剔除和裁剪各自发生在哪个空间下？

- 背面剔除：观察空间（view space），规定三角形顶点的环绕顺序（比如逆时针为正面），将顶点变换到视角空间后，如果有相反顺序（比如顺时针）则认为是背面

    参考：[面剔除 - LearnOpenGL CN (learnopengl-cn.github.io)](https://learnopengl-cn.github.io/04 Advanced OpenGL/04 Face culling/#_2)

- 裁剪：裁剪空间，坐标的范围为$$[-w,w]$$，在范围之外的三角形会被裁剪或者丢弃

### 光栅化的方法，画线的方法

### 法线贴图如何实现的，法线贴图作用

顶点不可能足够精细，而最终反映颜色的最小单位是片元，可以利用采样贴图的方式得到片元的额外信息，比如漫反射贴图可以用于表示片元（模型表面上某个点）的基本颜色，而法线贴图同理，可以表示片元的法线方向

作用：高模转低模，将高质量的法线信息保存到法线贴图上，能够减少顶点数量但不丢失细节

### PBR了解嘛？最常用PBR如何组成？

### Unity的StandardShader为什么手游中不提倡使用？为什么性能损耗严重？

变体太多，增加内存占用和加载shader资源的时间

针对桌面端而非移动端，变量精度较高，使用PBR的方式计算消耗较大

推荐使用简化版本的着色器：`Mobile->Lit`等（使用简化的光照模型）

### 自己实现过阴影嘛？如何实现的？

平面阴影：王者荣耀的实现，关键点是计算mesh的各个顶点在所需平面上的交点，以及考虑用模板测试进行阴影的渐变

ShadowMap：

- 从摄像机（视角）方向出发，仅记录每个片元的深度值，存储到**与屏幕分辨率匹配**的场景的深度纹理中
- 从光源方向出发，仅将深度存储到灯光的深度纹理中
- 将第一个步骤中得到的裁剪空间下的片元转换到光源空间，其深度值也被转换
- Hidden/Internal-ScreenSpaceShadows.shader逐片元地分别从场景和灯光的深度纹理中采样，比较两者的深度值大小，如果场景深度值>灯光深度值，说明未照亮，否则照亮。并将最终阴影值渲染到屏幕空间阴影贴图（照亮则纹素设置为1，未照亮则为0）
- 采样阴影贴图，并渲染

### 渲染管线流程，MVP每个矩阵的处理位置和空间概念，矩阵运算拷问

- 渲染流水线：[Unity-Shader-Note/渲染流水线.md at master · YuzikiRain/Unity-Shader-Note (github.com)](https://github.com/YuzikiRain/Unity-Shader-Note/blob/master/渲染流水线.md)

- 空间变换：[Unity-Shader-Note/空间变换.md at master · YuzikiRain/Unity-Shader-Note (github.com)](https://github.com/YuzikiRain/Unity-Shader-Note/blob/master/空间变换.md)

### ZTest和模板测试发生在什么阶段

光栅化产生片元->片元着色器->**stencil test**->**z test**->alpha blend

### GBuffer 延迟渲染优缺点

优点：复杂度不会随着光源数量增加，消耗稳定，一般对于光源较多的场景来说性能更优

缺点：不能实现半透明、没有后处理、没有MSAA抗锯齿

### MSAA发生在什么阶段

### 描边实现

- 基于颜色检测的后处理：
    - 先渲染颜色缓冲到纹理，然后通过经验公式计算明度（luminance），卷积公式得出和周围像素的差异是否超过某个阈值来决定是否是边缘
    - 先用指定颜色（一般就纯色）渲染要描边的物体，然后
- 法线外扩：将顶点沿着法线方向移动一定距离（即描边宽度），再渲染成描边颜色，背面剔除设置为仅剔除正面来剔除前半部分的描边壳子，再利用深度测试剔除与物体重叠的部分
- 法线方向和视角方向的夹角判断是否是边缘：缺点是边缘宽度不能控制
- 模板测试：第一个pass写入模板缓冲（模板测试为always replace），第二个pass需要法线外扩，再进行模板测试，和第一个pass的模板值比较，仅当不相等时（即扩张出来的部分的片元）才渲染描边颜色
- SDF：还不太了解

### Blinn-phong公式

[Unity-Shader-Note/blinn-phong光照模型.md at master · YuzikiRain/Unity-Shader-Note (github.com)](https://github.com/YuzikiRain/Unity-Shader-Note/blob/master/blinn-phong光照模型.md)

### 深度值知道嘛？OpenGL如何开启？可以写入嘛？

-   线性深度：深度缓冲中使用非线性深度，如果需要可视化深度等操作，需要重新映射成线性深度，否则只有在非常靠近视锥体近平面的部分才能看到深度

### 设计缓存池保存顶点数据

### 全局的光线变化怎么实现的？

### 学习

对客户端开发的哪一块感兴趣？如何学习图形学？
最近看了哪些图形学方向的资料？简述
写过shader嘛？举例说明
自己GitHub上的项目，你的游戏引擎参考了什么？如何学习的OpenGL？
看你github写过渲染器和引擎，简述

## 网络

### 帧同步和状态同步的与表现，同步策略，回滚，补帧，快照，如何一致性

### 多线程服务器如何解决死锁问题

## lua

### 弱引用，弱表的试用

### ToLua XLua如何与C# C C++交互



## 物理



## 链接

-   [腾讯游戏客户端开发面试 (Unity + Cocos) 双重轰炸 社招6轮面试_EricBBB的博客-CSDN博客](https://blog.csdn.net/qq_37508511/article/details/104312992)
-   [网易游戏客户端开发工程师面试_EricBBB的博客-CSDN博客](https://blog.csdn.net/qq_37508511/article/details/104482624?spm=1001.2014.3001.5502)