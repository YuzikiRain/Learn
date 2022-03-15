- 默认没有应用`MaterialPropertyBlock`，使用Get或Set可以取得当前renderer应用的mpb

    ``` csharp
    var meshRenderer = GetComponent<MeshRenderer>();
    var mpb = new MaterialPropertyBlock();
    mpb.SetColor("_BaseColor", isGray ? Color.gray : Color.white);
    // 1.Set设置
    meshRenderer.SetPropertyBlock(mpb);
    // 2.Get
    meshRenderer.GetPropertyBlock(mpb);
    ```

- （已经设置MaterialPropertyBlock）每次修改后，需要使用SetPropertyBlock才会应用修改

    ``` csharp
    var meshRenderer = GetComponent<MeshRenderer>();
    meshRenderer.GetPropertyBlock(mpb);
    mpb.SetColor("_BaseColor", isGray ? Color.gray : Color.white);
    meshRenderer.SetPropertyBlock(mpb);
    ```

    

