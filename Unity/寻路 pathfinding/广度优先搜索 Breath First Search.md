### BFS
```
将起点加入到NodeQueue
CameFromDictionary[起点] = null;
while NodeQueue is not empty
	从NodeQueue中出队列下一个节点Current
	if (Current == 终点) return
    else
    访问周围节点NextNodes
	foreach Next in NextNodes
		if Next未被访问过（不在CameFromDictionary中）
		{ 将Next加入到NodeQueue中，CameFromDictionary[Next] = Current; }
```

### 重建路径

```
path = []
current = 终点
while (current != null)
{
	path.Add(current);
	current = CameFromDictionary[current];
}
```

