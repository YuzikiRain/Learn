``` csharp
// 获得slot
Slot slot = skeletonAnimation.Skeleton.FindSlot(slotName);
// 获得默认Skin下，名为slotName的slot下的所有attachment
List<SkinEntry> attachments = new List<SkinEntry>();
var slotIndex = skeletonAnimation.Skeleton.FindSlotIndex(slotName);
skeletonAnimation.Skeleton.Data.DefaultSkin.GetAttachments(slotIndex, attachments);
// 获得Slot的当前显示的Attachment
slot.Attachment
// 获得slot所在的bone
slot.BoneData
```

