```csharp
 yourComponent = yourComponent ?? GetComponent<yourComponentType>();
```
```csharp
 yourComponent = GetComponent<yourComponentType>();
```
使用类似这样的代码，在因为重新编译代码或修改了组件上的引用时，会触发OnValidate，并修改组件上的引用，并再次触发OnValidate