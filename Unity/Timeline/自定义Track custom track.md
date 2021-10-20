### 名词解释

#### TimelineAsset

-   每个Timeline实例下有若干个Track
-   每个TimelineAsset

#### 轨道TrackAsset

-   Timeline窗口下看到的每一行就是轨道

-   TrackBindingType标签指定了绑定于该Track类型（一般是继承了MonoBehaviour的组件，**对应PlayableBehaviour.ProcessFrame函数的最后一个参数object playerData**）

-   TrackClipType标签指定了Track内可创建哪一种Clip类型的实例

-   TrackColor标签指定了Track的颜色

- 每个轨道实例内可创建多个（同一类型的）Clip实例

-   ```csharp
    using UnityEngine.Timeline;
    
    [TrackBindingType(typeof(ControlledComponent))]
    [TrackClipType(typeof(DialogueControlClip))]
    [TrackColor(0f, 1f, 0f)]
    public class DialogueControlTrack : TrackAsset
    {
    }
    ```

#### 剪辑Clip（继承自PlayableAsset）  

- 初始化数据的方式

    - 序列化的PlayableBehaviour实例作为模板提供数据，clip本身并不包含任何数据。运行时会返回模板的拷贝，因此不会修改模板本身
    - clip本身提供序列化字段，并在CreatePlayable时将这些值传递给PlayableBehaviour进行初始化。
        推荐使用这种方式，**因为可以减少Inspector面板下的序列化PlayableBehaviour还需要再展开折叠一层，且清晰地要求对PlayableBehaviour进行初始化**

    

-   ```csharp
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    
    // 如果不添加这个，会报UnityException: Invalid type的错误
    // Apply this to a PlayableBehaviour class or field to indicate that it is not animatable.
    [NotKeyable]
    public class DialogueControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private DialogueControlBehaviour _template;
    
        public ClipCaps clipCaps => ClipCaps.None;
    
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            // 从模板拷贝ScriptableObject资源
            return ScriptPlayable<DialogueControlBehaviour>.Create(graph, _template);
        }
    }
    ```

#### PlayableBehaviour

-   包含了用于Clip的各种数据

-   ProcessFrame：Clip的“Update”，由使用者自己驱动其如何更新

-   ```csharp
    using System;
    using UnityEngine;
    using UnityEngine.Playables;
    
    [Serializable]
    public class DialogueControlBehaviour : PlayableBehaviour
    {
        [SerializeField] private string _dialogue;
        
        public void Init(object param1)
        {
            
        }
    
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var component = playerData as ControlledComponent;
            component.SetDialogue(_dialogue);
    
            float progress = (float)(playable.GetTime() / playable.GetDuration());
            component.SetDialogueProgress(progress);
    
            base.ProcessFrame(playable, info, playerData);
        }
    }
    ```

#### 基类PlayableAsset

-   Timeline、Track、Clip的基类都是PlayableAsset，之间是组合关系（比如一个TimelineAsset的outputs就是所有track），
-   GetTime()：当前Playable经过的时间
-   GetDuration()：表示Playable的时长
-   要表示当前Playable经过的归一化时间，可以用GetTime() / GetDuration()

### Clip的混合

-   Track返回自定义的Mixer

    ```csharp
    using UnityEngine;
    using UnityEngine.Timeline;
    
    [TrackBindingType(typeof(ControlledComponent))]
    [TrackClipType(typeof(DialogueControlClip))]
    public class DialogueControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<CustomMixerPlayableBehaviour>.Create(graph, inputCount);
        }
    }
    ```

-   创建PlayableBehaviourMixer，**注意它跟目标PlayableBehaviour不是同一个，仅用于混合，也会触发ProcessFrame**

    ```csharp
    public class CustomPlayableBehaviourMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
    
            var inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<CustomPlayableBehaviour>)playable.GetInput(i);
                CustomPlayableBehaviour behaviour = inputPlayable.GetBehaviour();
                // 访问behaviour的各种字段属性，并进行混合
            }
        }
    }
    ```

### 从外部初始化自定义Track和Clip

``` csharp
// 这里的instantiatedPlayableAsset是读取的TimelineAsset进行Instantiate后的实例，否则会修改资产本身
var tracks = (instantiatedPlayableAsset as TimelineAsset).GetOutputTracks();
foreach (var track in tracks)
{
    var customTrack = track as CustomTrack;
    if (customTrack)
    {
        // track是否有clip
		bool hasCilps = trackAsset.hasClips;
        // 获得所有TimelineClip
        var timelineClips = moveTrack.GetClips();
        foreach (var timelineClip in timelineClips)
        {
            // 这里要取asset字段，因为默认是TimelineClip装着自定义Clip
            CustomClip moveClip = timelineClip.asset as CustomClip;
        }
    }
}
```



### 绑定TrackBindingType对应类型的实例

```csharp
PlayableAsset playableAsset;
foreach (var playableBinding in playableAsset.outputs)
{
    // 用类型 outputTargetType 或者用轨道名称 playableBinding.streamName 
    if (playableBinding.outputTargetType == typeof(GameObject))
    {
        playableDirector.SetGenericBinding(playableBinding.sourceObject, gameObject);
    }
}
```

### PlayableBehaviour生命周期

每个clip（继承自PlayableAsset类的实例）对应一个PlayableBehaviour（在clip实例被创建时调用clip的CreatePlayable函数返回的PlayableBehaviour实例）

PlayableDirector在到某个clip中时，FrameData的effectivePlayState为PlayState.Playing。在某个clip之外时，FrameData的effectivePlayState为PlayState.Paused

- OnPlayableCreate：PlayableDirector首次变为播放状态时，轨道上的**所有轨道的所有clip**所对应的PlayableBehaviour调用OnPlayableCreate（因为需要先创建PlayableGraph，再创建track实例和clip实例和clip对应的PlayableBehaviour，并将该PlayableBehaviour添加到PlayableGraph中）

- OnGraphStart：PlayableDirector变为播放状态时，轨道上的**所有轨道的所有clip**所对应的PlayableBehaviour调用OnGraphStart

- OnBehaviourPause：当PlayableDirector之前在某个clip中，当前在某个clip之外时，调用该clip所对应的PlayableBehaviour的OnBehaviourPause。

    **当PlayableDirector之前在某个clip中，PlayableDirector变为暂停状态时，也会调用OnBehaviourPause**
    **就算某个clip直接放在track的开头，也会先调用OnBehaviourPause再立即调用OnBehaviourPlay**

- OnBehaviourPlay：当PlayableDirector之前在clip之外，当前进入到某个clip中时，调用该clip所对应的PlayableBehaviour的OnBehaviourPlay

- PrepareFrame、ProcessFrame：当PlayableDirector进入到某个clip中时，每一帧会先调用该clip所对应的PlayableBehaviour的PrepareFrame，再调用ProcessFrame
    **最后一个参数object playerData表示Track所绑定的Component实例**

- OnGraphStop：PlayableDirector变为暂停状态（手动调用PlayableDirector.Pause()，编辑器里点暂停、播放完毕时会变为暂停）时，轨道上的**所有轨道的所有clip**所对应的PlayableBehaviour调用OnGraphStop

- OnPlayableDestroy：PlayableDirector的playableAsset播放完毕时（手动调用PlayableDirector.Stop()，即编辑器里看到进度条自动复位到0时），轨道上的**所有轨道的所有clip**所对应的PlayableBehaviour调用OnPlayableDestroy



### 使用FSM思想的PlayableBehaviour

``` csharp
using UnityEngine.Playables;

namespace BordlessFramework.PluginExtension
{
    public class CustomPlayableBehaviour : PlayableBehaviour
    {
        public double Time => Playable.GetTime();
        public double Duration => Playable.GetDuration();
        public double Progress => Time / Duration;

        public bool IsPlaying { get; private set; }
        public bool IsInClip { get; private set; }
        protected Playable Playable;

        private bool IsComponentCached = false;

        /// <summary>
        /// 首次进入时调用（Editor）
        /// </summary>
        /// <param name="playerData"></param>
        internal void ReInitInEditor(object playerData) { }

        public override void OnPlayableCreate(Playable playable)
        {
            Playable = playable;
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            OnDestroy();
        }

        public override void OnGraphStart(Playable playable)
        {
            IsPlaying = true;
        }

        public override void OnGraphStop(Playable playable)
        {
            IsPlaying = false;
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (!IsInClip)
            {
                IsInClip = true;
                OnEnter();
            }
            else
                IsInClip = true;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            // PlayableDirector暂停先触发OnBehaviourPause，再触发OnGraphStop，所以此时IsPlaying仍为true
            if (IsInClip && IsPlaying)
            {
                IsInClip = false;
                OnExit();
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!IsComponentCached)
            {
                Awake(playerData);
                IsComponentCached = true;
            }
            OnStay();
        }

        /// <summary>
        /// 首次进入时调用（Player）
        /// <para>在这里进行Track绑定的组件的实例的缓存</para>
        /// <paramref name="trackBindTypeInstance"/>Track绑定的组件的实例
        /// </summary>
        /// <param name="playerData"></param>
        protected virtual void Awake(object trackBindTypeInstance) { }
        /// <summary>
        /// 进入时调用
        /// </summary>
        protected virtual void OnEnter() { }
        /// <summary>
        /// 在timeline中时，每帧调用
        /// </summary>
        protected virtual void OnStay() { }
        /// <summary>
        /// 退出时调用
        /// </summary>
        protected virtual void OnExit() { }
        /// <summary>
        /// 销毁时调用
        /// </summary>
        protected virtual void OnDestroy() { }

    }
}
```

