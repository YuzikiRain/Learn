``` csharp
// 获得默认Skin下，名为slotName的slot下的所有attachment
List<SkinEntry> attachments = new List<SkinEntry>();
var slotIndex = skeletonAnimation.Skeleton.FindSlotIndex(slotName);
skeletonAnimation.Skeleton.Data.DefaultSkin.GetAttachments(slotIndex, attachments);

```

