如果出现远程分支上明明没有某个文件夹，本地是missing（删除）状态，重置时提示无法恢复xxx而不恢复孩子，可以尝试以下方法（先提交需要提交的文件，这个操作会重置整个文件夹）

- 重新安装SVN

![image-20220418191939783](https://cdn.jsdelivr.net/gh/YuzikiRain/ImageBed/img/image-20220418191939783.png)

- 打开cmd命令提示符，执行以下脚本：`cd /d 工程目录`
- 执行以下脚本：`svn revert . --depth infinity`