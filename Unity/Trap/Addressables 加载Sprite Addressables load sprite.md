### 找不到资源的BUG

```csharp
Addressables.LoadAssetAsync("folder/sprite.png");
```
假设你的sprite在Addressables中的地址为```folder/sprite.png```
当使用版本大于1.3.8的Addressables时，在使用非Use Exsisting Build的模式时，无法找到资源。
但当你的sprite的地址为```sprite.png```，不带任何父目录时，则不会出现以上问题。

### 正确的使用方法
- 使用版本1.3.8（或者更低版本，目前1.3.8 < 版本 <= 1.7.4的均存在该BUG）
-   ```csharp
    Addressables.LoadAssetAsync("folder/sprite.png[sprite]");
    // 对于sprite atlas中只有一个sprite的情形，最后的[sprite]是可以省略的，以下代码也有效
    Addressables.LoadAssetAsync("folder/sprite.png");
    // 对于sprite atlas中有多个sprite的情形，最后的[sprite_0]不能省略，[sprite_0]为sprite atlas中的目标sprite名称
    Addressables.LoadAssetAsync("folder/sprite.png[sprite_0]");
    ```