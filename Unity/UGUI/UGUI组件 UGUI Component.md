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

