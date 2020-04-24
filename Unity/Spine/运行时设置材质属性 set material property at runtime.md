
``` csharp
private UnityEngine.MaterialPropertyBlock _block;

void On()
{
    int value = 1;
    _block = new UnityEngine.MaterialPropertyBlock();
    _block.SetInt("propertyName in your shader", value);
    GetComponent<UnityEngine.MeshRenderer>().SetPropertyBlock(_block);
}
void Off()
{
    _block.Clear();
    GetComponent<UnityEngine.MeshRenderer>().SetPropertyBlock(_block);
}
```

### 参考
[Pharan from Unity Official Forum](http://zh.esotericsoftware.com/forum/Custom-Material-Shader-on-Spine-Animation-5906)