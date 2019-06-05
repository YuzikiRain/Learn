
### 步骤
- 父物体（空）命名为SpineTest，子物体A为从Project窗口中拖放SkeletonData Asset到Hierarchy窗口实例化生成的物体，子物体HitBox（空）
- 为子物体HitBox添加组件 BoundingBoxFollower ，设置SkeletonRenderer引用为子物体A上的组件，Slot为BoundingBox所在的Bone，并勾选IsTrigger（防止将物体撞开）
- 点击组件 BoundingBoxFollower 上的按钮 Add Bone Follower，生成 BoneFollower 组件，保持默认设置（组件的Bone Name自动设置为BoundingBox所在的Bone的父骨骼

## 参考
[Spine Keyed Bounding Box Attachments from youtube](https://www.youtube.com/watch?v=boIJm1o8Pkw)
