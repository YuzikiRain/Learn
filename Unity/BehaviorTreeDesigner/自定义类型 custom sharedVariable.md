```csharp
[System.Serializable]
public class SharedOBJECT_TYPE : SharedVariable<OBJECT_TYPE>
{
   public static implicit operator SharedOBJECT_TYPE(OBJECT_TYPE value) { return new SharedOBJECT_TYPE { Value = value }; }
}
```

参考：[Creating Shared Variables - Opsive](https://opsive.com/support/documentation/behavior-designer/variables/creating-shared-variables/)