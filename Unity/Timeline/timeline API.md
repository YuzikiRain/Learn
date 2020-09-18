```csharp
// 从timeline资源中取得各个Track的信息
foreach (var playable in playableAsset.outputs)
{
    // track实例的名称，可以在timeline面板上看到并修改
    var trackName = playable.streamName;
    // instanceOfTrackBindingType就是Track的标签中的类型的实例，一般是Unity Component
	playableDirector.SetGenericBinding(playable.sourceObject, instanceOfTrackBindingType);
}

[TrackBindingType(typeof(instanceOfTrackBindingType))]
public class CustomTrack : TrackAsset
{
}
```

