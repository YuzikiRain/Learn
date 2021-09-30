```csharp
// 从timeline资源中取得各个Track的信息
var playableAsset = playableDirector.playableAsset;
// 绑定
foreach (var playableBinding in playableAsset.outputs)
{
    // track实例的名称，可以在timeline面板上看到并修改
    var trackName = playableBinding.streamName;
    // instanceOfTrackBindingType就是Track的标签中的类型的实例，一般是Unity Component
	playableDirector.SetGenericBinding(playableBinding.sourceObject, instanceOfTrackBindingType);
}

// 获得索引为index的Track的信息
TrackAsset trackAsset = (playableAsset as TimelineAsset).GetOutputTrack(index);
var tracks = (playableAsset as TimelineAsset).GetOutputTracks();
foreach (var track in tracks)
{
    var customTrack = track as CustomTrack;
    if (customTrack)
    {
        // track是否有clip
		var hasCilps = trackAsset.hasClips;
        // 获得所有TimelineClip
        var timelineClips = moveTrack.GetClips();
        foreach (var timelineClip in timelineClips)
        {
            // 这里要取asset字段，因为默认是TimelineClip装着自定义Clip
            MoveClip moveClip = timelineClip.asset as CustomClip;
            moveClip.self = self;
            moveClip.to = target;
        }
    }
}

[TrackBindingType(typeof(instanceOfTrackBindingType))]
public class CustomTrack : TrackAsset
{
}
```

