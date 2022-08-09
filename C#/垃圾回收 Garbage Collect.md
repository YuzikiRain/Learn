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

常见的有引用计数（reference counting）和mark sweep

### mark

从应用程序的root开始，遍历托管堆上的所有对象，将其标记为非活动的（需要被删除）

从这些对象的字段引用继续遍历，直到所有对象都被检查完毕。

如何避免标记时因为循环引用而造成死循环：如果一个对象已经被标记，则不再检查其字段



