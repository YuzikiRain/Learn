## （序列化在）物体上的行为树

执行节点`Start Behavior Tree`或`Restart Behavior Tree`，效果是执行物体的**首个**`Behavior Tree`组件的group为指定值的行为树

- Behavior GameObject：如果要执行同一个物体上的行为树则留空，否则填对应行为树所在物体

- group：由于添加的`Behavior Tree`组件的group默认都是0，如果是要执行同一个物体的行为树，应该要把对应`Behavior Tree`组件的group改为其他非零值，否则无法执行（见如下代码，只会执行对应物体上首个对应group的行为树)
    ``` c#
    for (int i = 0; i < behaviorTrees.Length; ++i) 
    {
        if (behaviorTrees[i].Group == group.Value) 
        {
            behavior = behaviorTrees[i];
            break;
        }
    }
    ```

- Synchronize Variables：将父行为树的变量拷贝到执行的子行为树上，会覆盖同名变量

## 外部行为树

执行节点`Behavior Tree Reference`

- External Behaviors：要执行的外部行为树
- Variables：
    - 默认会将父行为树的变量拷贝到执行的子行为树上，会覆盖同名变量
    - 之后拷贝自定义的变量Variables，会覆盖同名变量

参考：[External Behavior Trees - Opsive](https://opsive.com/support/documentation/behavior-designer/external-behavior-trees/)

