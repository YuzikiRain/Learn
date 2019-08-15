- 使用 GetComponent<Renderer>().sharedMaterial 会报错 **“Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.”**，且所使用的Material后边会显示(Instance)

- 正确做法是使用 GetComponent<Renderer>().sharedMaterial
``` csharp
// create different material(not instance but like instance, serialized in scene)
_material = new Material(Shader.Find("Custom/YourShaderName"));
_material.SetTexture("_NormalMap", texture);
GetComponent<Renderer>().sharedMaterial = _material;
```

```
Shader "Custom/YourCustomShader" {
  Properties {
```
