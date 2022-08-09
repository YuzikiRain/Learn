lua的弱引用就是用弱表实现的

一般而言，table的key和value都是**强引用（strong reference）**的，这会阻止垃圾回收器对其的