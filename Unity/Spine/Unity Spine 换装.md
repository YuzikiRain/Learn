## 更新
Spine3.8版本中增加了新的API可以更方便地切换Attachment
``` csharp
var slotName = "weapon";
var attachmentName = "sword2";
GetComponent<SkeletonAnimation>().Skeleton.SetAttachment(slotName, attachmentName);
```

环境：Spine3.7

### 结构
- ```Atlas List<AtlasRegion> regions```
- ```Skeleton ExposedList<Slot> slots```

### 获得Skeleton
```csharp
GetComponent<SkeletonMecanim>().Skeleton;
GetComponent<SkeletonAnimation>().Skeleton;
GetComponent<skeletonRenderer>().Skeleton;
```

### 获得Slot
```csharp
var slotName = "weapon";
var skeleton = GetComponent<SkeletonMecanim>().Skeleton;
skeleton.FindSlot(slotName);
```

### 更换Attachment
- 通过Atlas获得Region，并替换Attachment（官方案例）
```csharp
public static void ReplaceAttachmentFromAtlas(this SkeletonRenderer skeletonRenderer, string slotName, SpineAtlasAsset atlasAsset, string regionName)
{
    var slot = skeletonRenderer.Skeleton.FindSlot(slotName);
    var originalAttachment = slot.Attachment;
    var atlas = atlasAsset.GetAtlas();
    var region = atlas.FindRegion(regionName);
    var scale = skeletonRenderer.skeletonDataAsset.scale;
    if (originalAttachment != null)
    {
        slot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);
    }
    else
    {
        var newRegionAttachment = region.ToRegionAttachment(region.name, scale);
        slot.Attachment = newRegionAttachment;
    }
}
```
- 直接使用Texture2D或Sprite创建Attachment并进行替换
```csharp
using Spine.Unity.Modules.AttachmentTools;
public static RegionAttachment ReplaceAttachmentFromSprite(this Skeleton skeleton, string slotName, Sprite sprite, Shader shader, bool applyPMA, float rotation = 0f)
{
    RegionAttachment att = applyPMA ? sprite.ToRegionAttachmentPMAClone(shader, rotation: rotation) : sprite.ToRegionAttachment(new Material(shader), rotation: rotation);
    skeleton.FindSlot(slotName).Attachment = att;
    return att;
}
```

## 参考
[简书 PA_ Spine使用图片换装 - Unity](https://www.jianshu.com/p/893cc1dd2838)
