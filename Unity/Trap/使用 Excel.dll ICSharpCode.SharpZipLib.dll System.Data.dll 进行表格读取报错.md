查看Edit -> Project Settings -> Player -> Api Compatibility Level，是否设置为.NET Standard 2.0，如果是则可以使用以下方法。
- 方法一：将 “your Unity install folder/Editor/Data/Mono/lib/mono/System.Data.dll”库文件添加到vs工程中（解决方案管理器 -> 添加引用）
- **方法二（推荐）：设置改为 .NET 4.x**