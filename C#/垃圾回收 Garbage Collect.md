## GC

``` c#
class A
{
    public B b;
}

class B
{
    public A a;
}

void main()
{
    A a = new A();
    a.b = new B();
    B b = new B();
    b.a = new A();
    A a2 = new A();
    B b2 = new B();
   
    // 标记A
    a.b = null;
    // 标记B
    b.a.b = null;
    // 标记C
    b.a = null;
    // 标记D
    a2 = null;
    // 标记E
}
```

程序运行后，在标记A处，所有对象都是可达的（reachable）

``` mermaid
graph
root(root)-->a
a((a))-->a.b
a.b((a.b))
root(root)-->b
b((b))-->b.a
b.a((b.a))-->b
root(root)-->a2
a2((a2))
root(root)-->b2
b2((b2))
```

在标记B处，a.b变为不可达（unreachable）

``` mermaid
graph
root(root)-->a
a((a))-.->a.b
a.b((a.b))
root(root)-->b
b((b))-->b.a
b.a((b.a))-->b
root(root)-->a2
a2((a2))
root(root)-->b2
b2((b2))
style a.b fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
```

在标记C处，b.a.b指向null，但b.a仍可达

``` mermaid
graph
root(root)-->a
a((a))-.->a.b
a.b((a.b))
root(root)-->b
b((b))-->b.a
b.a((b.a))-.->b
root(root)-->a2
a2((a2))
root(root)-->b2
b2((b2))
style a.b fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
```

在标记D处，b.a指向null，此时b.a不可达

``` mermaid
graph
root(root)-->a
a((a))-.->a.b
a.b((a.b))
root(root)-->b
b((b))-.->b.a
b.a((b.a))-.->b
root(root)-->a2
a2((a2))
root(root)-->b2
b2((b2))
style a.b fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
style b.a fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
```

在标记E处，a2指向null，此时a2不可达

``` mermaid
graph
root(root)-->a
a((a))-.->a.b
a.b((a.b))
root(root)-->b
b((b))-.->b.a
b.a((b.a))-.->b
root(root)-.->a2
a2((a2))
root(root)-->b2
b2((b2))
style a.b fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
style b.a fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
style a2 fill:000,stroke:#f66,stroke-width:4px,stroke-dasharray: 5, 5
```

若此时进行GC，则a.b，b.a，a2这三个对象都会被回收

## mark sweep

常见的垃圾回收算法有标记清除（mark sweep）和引用计数（reference counting）

### mark

只关心引用类型的变量，因为只有这种变量才能引用堆上的对象。值类型变量直接包含值类型实例，而不是指向堆上的一个引用

首先暂停进程中的所有线程，防止线程在CLR检查期间访问对象并更改其状态

从应用程序的root开始，遍历托管堆上的所有对象，将其标记为非活动的（需要被删除）

从这些对象的字段引用继续遍历，直到所有对象都被检查完毕。

**如何确保遍历所有对象：**

第一次从应用程序root开始将所有可达的对象加入队列

当队列不为空时，出队一个对象，将其标记为可达的。并访问其引用的字段，并加入到队列中

直到队列为空，则遍历了所有可达的对象

这种方法就是广度优先遍历

### sweep

直接向前移动所有活动对象，占据非活动对象内存空间，使其占用连续的内存空间。

然后移动托管堆的NextObjPtr指针指向最后一个活动对象之后的位置，下一个分配的对象将放在这个位置。

然后恢复所有线程。

**如何避免标记时因为循环引用而造成死循环：**如果一个对象已经被标记，则不再检查其字段

## 代（Generation）

