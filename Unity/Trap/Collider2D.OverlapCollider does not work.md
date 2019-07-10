碰撞体勾选了Is Trigger

### 使用contactFilter2D的默认初始化字段，无效

``` csharp
var contactFilter2D = new ContactFilter2D();

PolygonCollider2D attackBox = GetComponentInChildren<PolygonCollider2D>();
Collider2D[] polygonCollider2Ds = new Collider2D[10];
attackBox.OverlapCollider(contactFilter2D, polygonCollider2Ds);
```

**ContactFilter2D的useTriggers字段（以及所有bool字段）为 false**

### 设置ContactFilter2D.useTriggers = true后，起作用了

``` csharp
var contactFilter2D = new ContactFilter2D();
contactFilter2D.useTriggers = true;

PolygonCollider2D attackBox = GetComponentInChildren<PolygonCollider2D>();
Collider2D[] polygonCollider2Ds = new Collider2D[10];
attackBox.OverlapCollider(contactFilter2D, polygonCollider2Ds);
```

如果使用了layerMask，注意也要设置useLayerMask字段为 true

### 参考
[Unity Scripting API Collider2D.OverlapCollider](https://docs.unity3d.com/ScriptReference/Collider2D.OverlapCollider.html)
