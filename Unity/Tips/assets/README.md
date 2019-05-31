## 包含模块如下：

### Buff
- Buff: 从这个基类继承并创建自定义Buff，override其中的OnCreate，OnTrigger等方法来实现Buff的特殊效果
- BuffManager: 为IBuffable对象添加、删除Buff，以及驱动所有Buff的Update

### IO
- AssetManager: 资源管理
- JsonManager: 集成LitJson读取，从json文件读取数据并生成对应类型的实例对象

### UI
基于Unity的UGUI实现，使用纯代码创建并定制你的UI
- Window: 从这个基类继承并创建你的自定义窗口类
- UI: 包含UIPanel, UIButton, UIImage, UIText等基本UI要素

### Util
工具类
- Timer: 计时器工具，用于简单地在一定时间后执行某个操作，比手动写协程方法方便。使用System.DateTime.Now时间戳与当前时间比较，因此程序在后台时也会计时。依赖Plungins下的AsyncAwaitUtil
- Extensions: 扩展方法集合，如Vector的操作等。
