```csharp
 yourComponent = yourComponent ?? GetComponent<yourComponentType>();
```
```csharp
 yourComponent = GetComponent<yourComponentType>();
```
使用类似这样的代码，在因为重新编译代码或修改了组件上的引用时，会触发OnValidate，并修改（Project下的，即实际的预制体上的）组件上的引用，并再次触发OnValidate。而直接修改Hierarchy中的预制体则不会，因为此时需要Apply之后才会实际修改预制体。

为了避免这种情况，考虑使用以下代码
```csharp
 if (yourComponent == null) { yourComponent = GetComponent<yourComponentType>(); }
```