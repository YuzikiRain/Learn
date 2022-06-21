<img src="https://fastly.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220621105336819.png" alt="image-20220621105336819"  />

第一个Selector进行检测并设置状态变量isInTrigger，第二个Selector使用该变量

使用这种方式是因为第一个Selector里的任意一个Has Entered Trigger进入后，无法维持在后边的Idle节点上，会被另一个Has Entered Trigger中断（因为条件发生了变化，Has Entered Trigger只会在那一帧成功或失败）

[如何正确设置碰撞/触发器检测|操作性 (opsive.com)](https://opsive.com/forum/index.php?threads/how-to-properly-set-up-collision-trigger-detection.2130/#post-10373)