### API

``` csharp
// 请求刷新布局
LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
// 强制刷新布局（比如在这一帧添加了影响布局的子物体，需要马上刷新布局，以在这一帧取得某些子物体在新布局中的正确位置，否则布局下一帧才刷新，那得到的值就错误）
LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
// 强制更新RectTransform
rectTransform.ForceUpdateRectTransforms();
```

### Text

-   Horizontal Overflow
    -   Wrap（默认值）：到达水平边界时，文字将自动换行
    -   Overflow：文本可以超出水平边界
-   Vertical WrapMode
    -   Truncate（默认值）：到达垂直边界时，文本将被截断
    -   Overflow：文本可以超过垂直边界

Text 的 Vertical Overflow不会影响布局属性，  Horizontal Overflow会影响布局属性 preferred height

### Dropdown

```c#
for (int i = 0, length = _dropdown.options.Count; i < length; i++)
{
    var option = _dropdown.options[i];
    option.text = value;
}
// RefreshShownValue 会刷新当前项的CaptionText或CaptionImage
// 如果直接修改了options的text值或是设置option，并不会触发该函数。所以需要手动触发
RefreshShownValue();
```

