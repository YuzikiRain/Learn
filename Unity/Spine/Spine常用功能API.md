``` csharp
var skeleton = GetComponent<SkeletonAnimation>().Skeleton;
// 获得slot
Slot slot = skeletonn.FindSlot(slotName);
// 获得默认Skin下，名为slotName的slot下的所有attachment
List<SkinEntry> attachments = new List<SkinEntry>();
var slotIndex = skeleton.FindSlotIndex(slotName);
skeleton.Data.DefaultSkin.GetAttachments(slotIndex, attachments);
// 设置slot的attachment（spine3.8可用）
var slotName = "weapon";
var attachmentName = "sword2";
skeleton.SetAttachment(slotName, attachmentName);
// 获得Slot的*当前显示*的Attachment
var attachment = skeleton.GetAttachment(slotName, attachmentName);
slot.Attachment
// 是否没有显示任何Attachment（spine的显示至少是通过MeshAttachment之类的附件实现的）
slot.Attachment == null
// 获得slot所在的bone
slot.BoneData
// 获得指定名称的bone
skeleton.FindBone("boneName");
    
// 将所有slot重置到setup状态
skeleton.SetSlotsToSetupPose
// 将所有bone、constraint重置到setup状态
skeleton.SetBonesToSetupPose
// 将所有bone和slot重置到setup状态
skeleton.SetToSetupPose
// 移除所有轨道上的动画，但保持skeleton为当前pose
// 务必在SetToSetupPose之后调用该函数，因为轨道上仍有动画
skeleton.AnimationState.ClearTracks();

// 翻转
skeleton.FlipX = false
```

### 生命周期

![img](http://esotericsoftware.com/img/spine-runtimes-guide/spine-unity/spine-unity-skeletonanimation-updates.png)

### 层次结构

- skin->**bone->slot->attachment**（主要是MeshAttachment）
- .json或.skel文件：bone、slot、animation、ik、skins（其中attachment描述了如何生成对应mesh的信息）

### API手册

http://esotericsoftware.com/spine-api-reference
