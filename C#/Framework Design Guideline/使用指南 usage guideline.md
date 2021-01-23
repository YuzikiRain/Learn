### 数组

-   倾向于使用Collection而不是数组，
-   不要使用只读数组，因为数组元素仍可以被修改
-   尽可能使用锯齿数组而不是多维数组，锯齿数组在某些场景下（如表示稀疏矩阵）相比多维数组少浪费了一些内存空间，且CLR优化了锯齿数组的索引操作，因此在某些情况下它们可能表现出更好的运行时性能

### Attribute

-   尽可能seal自定义Attribute，这样可以更快地查找Attribute

### Collections

-   避免在public API中使用```List<T>```、Dictionary、Hashtable等，而是使用能够满足需求的它们实现的对应接口，因为这些类实现了过多的不必要的功能，大部分情况下使用者不会想要使用它们，这造成了更多困扰
-   考虑使用```ICollection<T>```或```IReadOnlyCollection<T>```的自定义派生类，这样可以在将来修改集合的实现或添加有帮助的方法
-   首先在实现了IEnumerator接口的GetEnumerator方法中使用IEnumerator作为返回类型，以确保foreach可用。除非很明确有其他需求，否在不要在其他方法中返回IEnumerator类型
-   使集合是只读的，否则用户可以直接绕过Add、Remove等方法来直接修改集合本身，这违背了Collections接口的设计初衷（对集合的操作仅通过这些接口来实现）。如果要替换集合中的多个元素，考虑添加AddRange方法
-   务必使用```ICollection<T>```作为表示读/写集合的属性或返回值，因为该接口基本满足了读（遍历，```ICollection<T>```实现了```IEnumerable<T>```）和写（```ICollection<T>```接口本身的Add、Remove等方法）的需求。除非不能满足需求（比如需要使用IList接口中的Insert方法），那么只好使用实现了```ICollections<T>```和其他对应需求的接口的自定义类型了。如果是只读集合，则换成```IReadOnlyCollection<T>```，如果是使用自定义只读集合，实现```ICollection<T>.IsReadOnly```并返回true
-   务必在public API中使用确切的类型来表示返回值和参数的集合元素，而不是使用基类。例如，不要使用public Animal Function(Animal animal) 而是 public Dog Function(Dog dog)
-   不要从属性或返回集合的方法中返回null，而是返回空集合或空数组，因为我们假定使用者不会检查返回的集合是否为null而是直接遍历集合内元素

#### 数组和Collection的选择

-   倾向于使用集合而不是数组

-   仅考虑在低级API中使用数组以优化性能

-   务必使用byte数组而不是byte集合

### 自定义集合

-   设计自定义集合时，考虑从```Collection<T>```，```ReadOnlyCollection<T>```或```KeyedCollection<TKey,TItem>```继承
-   避免在与集合概念无关仅为了提供一些集合操作的复杂API的类型上实现ICollection

### Equality operators

-   == 和 != 是成对的，如果要重载务必同时重载
-   确保==和 Object.Equals 具有完全相同的语义
-   值类型必须重载相等操作符，因为默认实现是不可靠的
-   对于引用类型，大部分语言都是默认使用引用相等。重载即表示通过其他方式（比如判断所有字段的避免为可变引用类型重载相等操作符，因为内置的相等操作符通常实现为判断，这也是大部分用户期望的默认结果。如果重载了这个默认行为，可能返回值会让用户摸不着头脑。对于不可变类型，重载倒是没多大问题，因为
-   如果重载实现比直接判断引用相等慢太多，应避免为引用类型进行相等运算符重载



​    

​    

