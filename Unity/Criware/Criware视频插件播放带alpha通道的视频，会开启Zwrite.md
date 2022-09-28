如果使用了`CriManaMovieController`而不是`CriManaMovieControllerForUI`，`RendererResource`会设置ZWrite为Off

``` c#
// RendererResource.cs
protected void SetupStaticMaterialProperties()
{
    // 这里如果使用了CriManaMovieController，ui为0，于是会打开ZWrite
    currentMaterial.SetInt("_ZWriteMode", ui ? 0 : 1);
    SetKeyword(currentMaterial, "CRI_ALPHA_MOVIE", hasAlpha);
}
```

如果是带alpha通道的视频，应该像半透明物体一样在透明队列中渲染，并关闭ZWrite

``` c#
// RendererResource.cs
protected void SetupStaticMaterialProperties()
{
    // 考虑了视频是否有Alpha通道
    currentMaterial.SetInt("_ZWriteMode", ui ? 0 : (hasAlpha ? 0 : 1));
    SetKeyword(currentMaterial, "CRI_ALPHA_MOVIE", hasAlpha);
}
```

