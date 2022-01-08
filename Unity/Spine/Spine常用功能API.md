``` csharp
// 获得slot
Slot slot = skeletonAnimation.Skeleton.FindSlot(slotName);
// 获得默认Skin下，名为slotName的slot下的所有attachment
List<SkinEntry> attachments = new List<SkinEntry>();
var slotIndex = skeletonAnimation.Skeleton.FindSlotIndex(slotName);
skeletonAnimation.Skeleton.Data.DefaultSkin.GetAttachments(slotIndex, attachments);
// 设置slot的attachment（spine3.8可用）
var slotName = "weapon";
var attachmentName = "sword2";
GetComponent<SkeletonAnimation>().Skeleton.SetAttachment(slotName, attachmentName);
// 获得Slot的当前显示的Attachment
slot.Attachment
// 获得slot所在的bone
slot.BoneData
    
// 将所有slot重置到setup状态
Skeleton.SetSlotsToSetupPose
// 将所有bone、constraint重置到setup状态
Skeleton.SetBonesToSetupPose
// 将所有bone和slot重置到setup状态
Skeleton.SetToSetupPose
// 移除所有轨道上的动画，但保持skeleton为当前pose
// 务必在SetToSetupPose之后调用该函数，因为轨道上仍有动画
Skeleton.AnimationState.ClearTracks();
```

### 生命周期

![img](http://esotericsoftware.com/img/spine-runtimes-guide/spine-unity/spine-unity-skeletonanimation-updates.png)

### 层次结构

- skin->**bone->slot->attachment**（主要是MeshAttachment）
- .json或.skel文件：bone、slot、animation、ik、skins（其中attachment描述了如何生成对应mesh的信息）

### API手册

http://esotericsoftware.com/spine-api-reference
