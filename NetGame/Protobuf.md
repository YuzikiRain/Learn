### Protoc.exe

- 进入到[google protobuf github release页面](https://github.com/protocolbuffers/protobuf/releases)，下载protoc-3.12.4-win64.zip
- 创建批处理文件
    ``` bat
        @echo off
    for %%i in (./proto/*.proto) do (
        rem --csharp_out=输出路径 输入路径
        protoc --csharp_out=./cs ./proto/%%i
        echo From %%i To %%~ni.cs Successfully!  
    )
    pause
    ```
- 该批处理以```./proto```目录下的所有.proto文件作为输入，在```./cs```目录输出.cs文件

### 添加dll
- 进入到[google protobuf github release页面](https://github.com/protocolbuffers/protobuf/releases)，下载对应C#语言的源码protoc-csharp-3.12.4.zip
- 打开```csharp/src/Google.Protobuf.sln```解决方案，然后点击 生成-重新生成解决方案
- 将```csharp/src/Google.Protobuf/bin/Debug/net45```目录下的所有dll文件拷贝到Unity工程下即可

### 常用API
``` csharp
public static byte[] Serialize<T>(T obj) where T : IMessage
{
    return obj.ToByteArray();
}

public static T Deserialize<T>(byte[] data, int offset, int length) where T : class, IMessage, new()
{
    T obj = new T();
    IMessage message = obj.Descriptor.Parser.ParseFrom(data, offset, length);
    return message as T;
}
```