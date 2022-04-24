帧率过低，或增大了`Time.timeScale`，会导致一次Update之后，经过了较长的时间片，`PlayableDirector`的当前时间`time`跳过了某些`Timeline Clip`，因此不会执行Clip对应的`Behaviour`的`ProcessFrame` `OnBehaviourPause` `OnBehaviourPlay` 等函数

## 解决方案

### 逐帧Evaluate

使用`PlayableDirectorWatcher.PlayAndWatchTimeline`来播放`TimelineAsset`，该class会以不低于`TimelineAsset`的默认帧率，逐帧地`Evaluate`整个`TimelineAsset`

``` csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace BordlessFramework
{
    /// <summary>
    /// 监视并让PlayableDirector逐帧播放TimelineAsset，确保不会帧率过低（deltaTime过大）跳过任何clip
    /// </summary>
    public static class PlayableDirectorWatcher
    {
        private static float frameRate = 60f;
        private static float interval = 1f / frameRate;
        private const float timelineAssetDefaultFrameRate = 30f;
        private static HashSet<PlayableDirector> playableDirectors = new HashSet<PlayableDirector>();
        private static HashSet<PlayableDirector> playableDirectorsToRemove = new HashSet<PlayableDirector>();

        static PlayableDirectorWatcher()
        {
            frameRate = Application.targetFrameRate;
            // 不低于TimelineAsset的默认帧率
            if (frameRate < timelineAssetDefaultFrameRate) frameRate = timelineAssetDefaultFrameRate;
            interval = 1f / frameRate;
            var obj = new GameObject();
            obj.hideFlags = HideFlags.HideAndDontSave;
            obj.AddComponent<PlayableDirectorWatcherUpdater>();
        }

        public static void PlayAndWatchTimeline(PlayableDirector playableDirector, TimelineAsset timelineAsset = null)
        {
            playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
            if (timelineAsset) playableDirector.Play(timelineAsset);
            else playableDirector.Play();
            Add(playableDirector);
        }

        private static void Add(PlayableDirector playableDirector)
        {
            playableDirectors.Add(playableDirector);
        }

        private static void Remove(PlayableDirector playableDirector)
        {
            playableDirectors.Remove(playableDirector);
        }

        class PlayableDirectorWatcherUpdater : MonoBehaviour
        {
            private void Update()
            {
                // 先删除待删除的
                foreach (var playableDirector in playableDirectorsToRemove)
                {
                    Remove(playableDirector);
                }
                playableDirectorsToRemove.Clear();

                // 更新现有的
                foreach (var playableDirector in playableDirectors)
                {
                    if (playableDirector.time >= playableDirector.playableAsset.duration)
                    {
                        playableDirector.Stop();
                    }
                    // playableDirector已经播放完毕并停止（仅适用于WrapMode为None的Director）
                    if (playableDirector.state == PlayState.Paused && playableDirector.time == 0d)
                    {
                        playableDirectorsToRemove.Add(playableDirector);
                        continue;
                    }

                    var previousTime = playableDirector.time;
                    while (true)
                    {
                        playableDirector.time += interval;
                        if (playableDirector.time > previousTime + Time.deltaTime)
                        {
                            playableDirector.time = previousTime + Time.deltaTime;
                            playableDirector.Evaluate();
                            break;
                        }
                        playableDirector.Evaluate();
                    }

                }
            }
        }
    }
}
```

 缺点：无法播放AudioTrack，该轨道必须要在非Manual模式下才能使用

### 逐帧检查当前Director时间是否超过Behaviour对应的TimelineClip的时间

需要使用TimelineClip的start和end初始化对应自定义clip，再初始化对应Behaviour

#### 初始化

``` csharp
var tracks = (playableAsset as TimelineAsset).GetOutputTracks().ToArray();
foreach (var track in tracks)
{
    switch (track)
    {
        case CustomTrack customTrack:
            {
                var timelineClips = customTrack.GetClips();
                foreach (var timelineClip in timelineClips)
                {
                    CustomClip customClip = timelineClip.asset as CustomClip;
                    customClip.SetPlayableDirector(playableDirector);
                    customClip.SetTime(timelineClip.start, timelineClip.end);
                }
                break;
            }
    }
}
```

#### WatchableClip 基类

``` csharp
using UnityEngine.Playables;

namespace BordlessFramework.PluginExtension
{
    public abstract class WatchableClip : PlayableAsset
    {
        protected PlayableDirector PlayableDirector;
        public double StartTime { get; private set; }
        public double EndTime { get; private set; }

        public void SetPlayableDirector(PlayableDirector playableDirector)
        {
            this.PlayableDirector = playableDirector;
        }

        public void SetTime(double startTime, double endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
```

#### CustomClip：你的自定义clip类

``` csharp
using BordlessFramework.PluginExtension;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CustomClip : WatchableClip
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.AddWatch(PlayableDirector, StartTime, EndTime);
    }
}
```

#### WatchableCustomPlayableBehaviour：基类

``` csharp
using UnityEngine.Playables;

namespace BordlessFramework.PluginExtension
{
    public class WatchableCustomPlayableBehaviour : PlayableBehaviour
    {
        public double StartTime { get; private set; }
        public double EndTime { get; private set; }
        public bool IsWatchStart = false;
        public bool IsWatchComplete = false;

        public virtual void OnWatchStart() { }
        public virtual void OnWatchComplete() { }

        public void AddWatch(PlayableDirector playableDirector, double startTime, double endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            PlayableWatcher.Add(playableDirector, this);
        }

        private void RemoveWatch()
        {
            PlayableWatcher.Remove(this);
        }
    }
}
```

#### CustomBehaviour：你的自定义Behaviour类

``` csharp
using BordlessFramework.PluginExtension;
using UnityEngine;

public class CustomBehaviour : WatchableCustomPlayableBehaviour
{
    public override void OnWatchStart()
    {
        // 自定义逻辑
    }
    
    public override void OnWatchComplete()
    {
        // 自定义逻辑
    }
}
```

#### PlayableWatcher：管理器，Updater

``` csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace BordlessFramework.PluginExtension
{
    /// <summary>
    /// 监视PlayableBehaviour是否播放开始、结束
    /// </summary>
    public static class PlayableWatcher
    {
        private static HashSet<WatchableCustomPlayableBehaviour> playableBehavioursToRemove = new HashSet<WatchableCustomPlayableBehaviour>();
        private static Dictionary<WatchableCustomPlayableBehaviour, PlayableDirector> playableBehaviourToDirector = new Dictionary<WatchableCustomPlayableBehaviour, PlayableDirector>();
        private static PlayableWatcherUpdater playableDirectorWatcherUpdater;

        static PlayableWatcher()
        {
            var obj = new GameObject();
            obj.hideFlags = HideFlags.HideAndDontSave;
            playableDirectorWatcherUpdater = obj.AddComponent<PlayableWatcherUpdater>();
        }

        public static void SetIsWatching(bool isWatching)
        {
            playableDirectorWatcherUpdater.enabled = isWatching;
        }

        public static void Reset()
        {
            playableBehavioursToRemove.Clear();
            playableBehaviourToDirector.Clear();
        }

        public static void Add(PlayableDirector playableDirector, WatchableCustomPlayableBehaviour customPlayableBehaviour)
        {
            playableBehaviourToDirector.Add(customPlayableBehaviour, playableDirector);
        }

        public static void Remove(WatchableCustomPlayableBehaviour customPlayableBehaviour)
        {
            playableBehavioursToRemove.Add(customPlayableBehaviour);
        }

        [DefaultExecutionOrder(9999)]
        class PlayableWatcherUpdater : MonoBehaviour
        {
            private void Update()
            {
                // 先删除待删除的
                foreach (var playableDirector in playableBehavioursToRemove)
                {
                    playableBehaviourToDirector.Remove(playableDirector);
                }
                playableBehavioursToRemove.Clear();

                // 更新现有的
                foreach (var pair in playableBehaviourToDirector)
                {
                    var playableBehaviour = pair.Key;
                    var playableDirector = pair.Value;

                    if (playableDirector.time >= playableBehaviour.StartTime)
                    {
                        if (!playableBehaviour.IsWatchStart)
                        {
                            playableBehaviour.IsWatchStart = true;
                            playableBehaviour.OnWatchStart();
                        }
                    }
                    if (playableDirector.time >= playableBehaviour.EndTime)
                    {
                        if (!playableBehaviour.IsWatchComplete)
                        {
                            playableBehaviour.IsWatchComplete = true;
                            playableBehaviour.OnWatchComplete();
                            Remove(playableBehaviour);
                        }
                    }

                }
            }
        }
    }
}
```

