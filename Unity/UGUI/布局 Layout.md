### 布局控制器
实现了ILayoutController的类型
常见的有 Horizontal/Vertical/Grid Layout Group。
至少有一个布局控制器，布局才会生效
（本身也实现了ILayoutElement，所以才会有布局的相关属性）

### LayoutElement
Image、Text等组件都实现了ILayoutElement接口，因此有这些布局属性
选中任意RectTransform物体，在Inspector面版下将预览窗口切换成Layout Properties，可以直接查看有关属性
- Min：布局生效时，子元素最小尺寸像素
- Preferred：布局生效时，子元素最大尺寸像素
- Flexible：表示占父UI尺寸的权重，即使未开启，也有默认值为1。布局生效时，布局控制器（即父物体）会收集所有子UI的Flexible权重，对应子UI的尺寸为 父UI尺寸 * 子UI权重 / 父UI权重

### 布局属性
Vertical Layout Group的Min和Preferred

|Control Child Size|宽度|高度|
|-|-|-|
|未勾选|子LayoutElement的 width 的最大值|子LayoutElement的 height 总和|
|勾选|子LayoutElement的 Min和Preferred 的最大值|子LayoutElement的 Min和Preferred 总和|
如果Padding不为0，那么Width要加上Left+Right，Height要加上Top+Bottom

##### 自动设置布局属性
**Image会自动设置Preferred为sprite的宽高像素**（如果在Sprite Editor里编辑了Border，那么会被实际设置成宽高减去border）
Text则与字体大小、字体、实际文本有关。

Flexible默认是不开启的

### Horizontal/Vertical Layout Group
- Child Force Expand：勾选则开启Flexible，并设置为1，不勾选则关闭
- Use Child Scale：

### Layout Element
- Ignore Layout：不受布局影响（对它来说就像是父UI没有布局组件一样，相当于将特定UI排除在布局之外，因为有时候不希望所有子物体都受布局影响，但又不得不做成子UI时就可以这么做）
- 其他属性：覆盖原本对应的Min/Preferred/Flexible
- Layout Priority：如果同一层级上有多个子UI都使用了Layout Element组件，那么这个优先级决定了哪个子UI的Layout Element最先被计算

### Content Size Filter

reference：
[UIFitContentSize from Unity Offical Manual](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/HOWTO-UIFitContentSize.html)