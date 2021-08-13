执行 ```git rebase --interactive``` 时

- **pick**: 保留commit以及commit信息
- **reword**: 可编辑该commit的信息（且不与其他commit合并）
- **squash**: 将commit和上一个commit合并，将信息添加到上一个（更旧的）非drop或fixup的commit的description的新行，最后必须要有一个reword（或edit）的commit来整合commit信息
- **fixup**: 将commit和上一个commit合并，但丢弃commit信息
- **drop**: 丢弃commit以及commit信息
- **edit**: 不了解。。。应该是等待执行amend后



### 在一条branch上合并多个commit信息

执行 ```git rebase --interactive```，将

