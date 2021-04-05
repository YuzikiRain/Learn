```csharp
// 从timeline资源中取得各个Track的信息
var playableAsset = playableDirector.playableAsset;
foreach (var playableBinding in playableAsset.outputs)
{
    // track实例的名称，可以在timeline面板上看到并修改
    var trackName = playableBinding.streamName;
    // instanceOfTrackBindingType就是Track的标签中的类型的实例，一般是Unity Component
	playableDirector.SetGenericBinding(playableBinding.sourceObject, instanceOfTrackBindingType);
}

// 获得索引为index的Track的信息
var trackAsset = (playableAsset as UnityEngine.Timeline.TimelineAsset).GetOutputTrack(index);
// track是否有clip
var hasCilps = trackAsset.hasClips;
// 获得所有TimelineClip
var timelineClips = trackAsset.GetClips();

[TrackBindingType(typeof(instanceOfTrackBindingType))]
public class CustomTrack : TrackAsset
{
}
```

