### 基本

#### TimelineAsset

这里指代后缀名为playable的timeline资产

-   每个Timeline实例下有若干个Track
-   每个TimelineAsset也是一种PlayableAsset

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

##### 生命周期

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

#### Clip之间的混合

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

#### 从外部初始化自定义Track和Clip

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
            CustomClip customClip = timelineClip.asset as CustomClip;
        }
    }
}
```

#### 绑定TrackBindingType对应类型的实例

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

#### 基类PlayableAsset

-   Timeline、Track、Clip的基类都是PlayableAsset，之间是组合关系（比如一个TimelineAsset的outputs就是所有track），注意Track的GetClips方法得到的是TimelineClip，其asset字段才是对应的自定义clip
-   GetTime()：当前Playable经过的时间
-   GetDuration()：表示Playable的时长
-   要表示当前Playable经过的归一化时间，可以用GetTime() / GetDuration()

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
		var hasCilps = customTrack.hasClips;
        // 获得所有TimelineClip
        var timelineClips = customTrack.GetClips();
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

### 高级

#### `ExposedReference<Object>`

本质是键值对，实现了IExposedPropertyTable接口的PlayableDirector保存了一组键值对

可使得TimelineAsset引用场景中的资源，便于编辑器下预览。运行时则可以动态设置其引用

参考：[Unity - Scripting API: ExposedReference (unity3d.com)](https://docs.unity3d.com/ScriptReference/ExposedReference_1.html)

##### 设置引用

-   引用序列化在Clip里了，直接在clip里设置（这种情况下Director组件物体和引用都在同一场景里）

    ``` csharp
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    
    [NotKeyable]
    public class TestClip : PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        public ExposedReference<Transform> fromReference;
        public ExposedReference<Transform> toReference;
    }
    ```

-   在外部通过PlayableDirector设置引用

    ``` csharp
    PlayableDirector playableDirector;
    var tracks = (playableAsset as TimelineAsset).GetOutputTracks();
    foreach (var track in tracks)
    {
        var customTrack = track as CustomTrack;
        if (customTrack)
        {
            // track是否有clip
    		var hasCilps = customTrack.hasClips;
            // 获得所有TimelineClip
            var timelineClips = customTrack.GetClips();
            foreach (var timelineClip in timelineClips)
            {
                // 这里要取asset字段，因为默认是TimelineClip装着自定义Clip
                TestClip customClip = timelineClip.asset as TestClip;
                playableDirector.SetReferenceValue(fromReference.exposedName, objectReference);
            }
        }
    }
    ```

##### 获取引用

-   在Clip里

``` csharp
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[NotKeyable]
public class TestClip : PlayableAsset, ITimelineClipAsset, IPropertyPreview
{
    public ExposedReference<Transform> fromReference;
    public ExposedReference<Transform> toReference;

    [SerializeField] private TestBehaviour _template;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var resolver = graph.GetResolver();
        var from = fromReference.Resolve(resolver);
        var to = toReference.Resolve(resolver);
        // 从模板拷贝ScriptableObject资源
        var playable = ScriptPlayable<TestBehaviour>.Create(graph, _template);
        playable.GetBehaviour().Init(from, to);
        return playable;
    }
}
```

Resolve是包装了GetReference的泛型方法，一般使用Resolve而不直接用GetReferencce

```csharp
public T Resolve(IExposedPropertyTable resolver)
{
    if (resolver != null)
    {
        bool idValid;
        Object referenceValue = resolver.GetReferenceValue(exposedName, out idValid);
        if (idValid)
        {
            return referenceValue as T;
        }
    }

    return defaultValue as T;
}
```

#### 预览修改（退出预览则复原）

-   Track

    ``` csharp
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(TestClip))]
    [TrackColor(0f, 1f, 0f)]
    public class TestTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
    #if UNITY_EDITOR
        	var comp = director.GetGenericBinding(this) as Transform;
    
            if (comp == null) return;
            var so = new UnityEditor.SerializedObject(comp);
    
            var iter = so.GetIterator();
    
            while (iter.NextVisible(true))
            {
                if (iter.hasVisibleChildren) continue;
                driver.AddFromName<Transform>(comp.gameObject, iter.propertyPath);
            }
    #endif
            base.GatherProperties(director, driver);
        }
    }
    ```
    
-   Clip

    ``` csharp
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    
    [NotKeyable]
    public class TestClip : PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        [SerializeField] private TestBehaviour _template;
    
        public ClipCaps clipCaps => ClipCaps.None;
    
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<TestBehaviour>.Create(graph, _template);
        }
    
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            const string kLocalPosition = "m_LocalPosition";
            const string kLocalRotation = "m_LocalRotation";
            driver.AddFromName<Transform>(kLocalPosition + ".x");
            driver.AddFromName<Transform>(kLocalPosition + ".y");
            driver.AddFromName<Transform>(kLocalPosition + ".z");
            driver.AddFromName<Transform>(kLocalRotation + ".x");
            driver.AddFromName<Transform>(kLocalRotation + ".y");
            driver.AddFromName<Transform>(kLocalRotation + ".z");
            driver.AddFromName<Transform>(kLocalRotation + ".w");
        }
    }
    ```

    适用于同一个Track内的不同Clip之间有不同的预览属性（都相同则可以直接在Track里写）

参考：https://forum.unity.com/threads/temporary-preview-changes-from-custom-timeline-track.650902/

#### 取得ExposedReference对应的值

- Clip中取得

    ``` csharp
    public class TestClip : PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        [SerializeField] private TestBehaviour _template;
    
        public ClipCaps clipCaps => ClipCaps.None;
    
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<TestBehaviour>.Create(graph, _template);
        }
    }
    ```

- 激活TimelineEditorWindow编辑器时

    - 从自定义编辑器扩展中

        ``` csharp
        [UnityEditor.CustomEditor(typeof(ParticleControlClip))]
        public class ParticleControlClipEditor : UnityEditor.Editor
        {
            private void OnEnable()
            {
                // 直接取得序列化字段exposedReference的exposedReferenceValue值，就是需要的值
                var sourceGameObjectProperty = this.serializedObject.FindProperty("sourceGameObject");
        		var instance = sourceGameObjectProperty.exposedReferenceValue;
            }
        }
        ```

#### 取得PlayableDirector

- 激活TimelineEditorWindow编辑器时

    - 从自定义编辑器扩展中：serializedObject.context

        ``` csharp
        [UnityEditor.CustomEditor(typeof(ParticleControlClip))]
        public class ParticleControlClipEditor : UnityEditor.Editor
        {
            private void OnEnable()
            {
                var director = this.serializedObject.context as PlayableDirector;
            }
        }
        ```

    - `UnityEditor.Timeline.TimelineEditor.inspectedDirector`

    - `var directorContext = UnityEditor.Selection.activeContext as PlayableDirector;`
        未经过验证，来自julienb [How can I resolve an ExposedReference from within a Marker? - Unity Forum](https://forum.unity.com/threads/how-can-i-resolve-an-exposedreference-from-within-a-marker.1043728/)

### 自定义

#### 使用FSM思想的PlayableBehaviour

``` csharp
using UnityEngine.Playables;

namespace BordlessFramework.PluginExtension
{
    public class CustomPlayableBehaviour : PlayableBehaviour
    {
        public double Time => Playable.GetTime();
        /// <summary>
        /// 在OnEnter或Awake
        /// </summary>
        public double Duration => Playable.GetDuration();
        public double Progress => Time / Duration;

        public bool IsPlaying { get; private set; }
        public bool IsInClip { get; private set; }
        protected Playable Playable;

        private bool IsComponentCached = false;

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

        //调用顺序如下

        /// <summary>
        /// 进入时调用
        /// </summary>
        protected virtual void OnEnter() { }
        /// <summary>
        /// 首次进入时调用（Player），注意其在OnEnter之后
        /// <para>在这里进行Track绑定的组件的实例的缓存</para>
        /// <paramref name="trackBindTypeInstance"/>Track绑定的组件的实例
        /// </summary>
        /// <param name="playerData"></param>
        protected virtual void Awake(object trackBindTypeInstance) { }
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

#### sin函数实现屏幕震动

```csharp
using BordlessFramework.PluginExtension;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeBehaviour : CustomPlayableBehaviour
{
    private Transform cameraTransform;
    private bool isRandom;
    private Vector3 direction;
    private AnimationCurve amplitude;
    private float frequency;
    private float scale;
    private Dictionary<int, Vector3> randomDirections = new Dictionary<int, Vector3>();

    public void Init(Transform cameraTransform, bool isRandom, Vector3 direction, AnimationCurve amplitude, float frequency, float scale)
    {
        this.cameraTransform = cameraTransform;
        this.isRandom = isRandom;
        this.direction = direction;
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.scale = scale;
    }

    protected override void Awake(object trackBindTypeInstance)
    {
        // 生成随机方向数组
        if (isRandom)
        {
            for (int i = 0; i < (float)Duration * frequency; i++)
            {
                randomDirections.Add(i, (Quaternion.AngleAxis(Random.Range(0f, 180f), Vector3.forward) * Vector3.right));
            }
        }
    }

    protected override void OnStay()
    {
        // 每个周期从已经生成好的随机方向数组中取得新的随机方向
        if (isRandom) direction = randomDirections[(int)(Time * frequency)];
        cameraTransform.transform.position = Mathf.Sin((float)Time * frequency * 2f * Mathf.PI) * amplitude.Evaluate((float)Progress) * direction * scale;
    }

    protected override void OnExit()
    {
        cameraTransform.transform.position = Vector3.zero;
    }
}

```

#### Clip对应多个PlayableBehaviour

参考PlayableControlAsset

``` csharp
public class CustomClip : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playables = new List<Playable>();
        var playableA = PlayableA.Create(graph, prefabGameObject, parenTransform);
        var playableB = PlayableB.Create(graph, prefabGameObject, parenTransform);
        playables.Add(playableA);
        playables.Add(playableB);
        return ConnectPlayablesToMixer(graph, playables);
    }
    
    static Playable ConnectPlayablesToMixer(PlayableGraph graph, List<Playable> playables)
    {
        var mixer = Playable.Create(graph, playables.Count);

        for (int i = 0; i != playables.Count; ++i)
        {
            ConnectMixerAndPlayable(graph, mixer, playables[i], i);
        }

        mixer.SetPropagateSetTime(true);

        return mixer;
    }
    
    static void ConnectMixerAndPlayable(PlayableGraph graph, Playable mixer, Playable playable,
	 int portIndex)
    {
        graph.Connect(playable, 0, mixer, portIndex);
        mixer.SetInputWeight(playable, 1.0f);
    }
}
```

#### 粒子播放完成后自动销毁

#### ParticleControlTrack

``` csharp
using UnityEngine.Timeline;

[TrackClipType(typeof(ParticleControlClip))]
public class ParticleControlTrack : TrackAsset
{

}
```

##### ParticleControlClip

修改自PlayableControlAsset

``` csharp
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
    /// <summary>
    /// Playable Asset that generates playables for controlling time-related elements on a GameObject.
    /// </summary>
    [Serializable]
    [NotKeyable]
    public class ParticleControlClip : PlayableAsset, IPropertyPreview, ITimelineClipAsset
    {
        const int k_MaxRandInt = 10000;
        static readonly List<PlayableDirector> k_EmptyDirectorsList = new List<PlayableDirector>(0);
        static readonly List<ParticleSystem> k_EmptyParticlesList = new List<ParticleSystem>(0);
        static readonly HashSet<ParticleSystem> s_SubEmitterCollector = new HashSet<ParticleSystem>();

        /// <summary>
        /// GameObject in the scene to control, or the parent of the instantiated prefab.
        /// </summary>
        [SerializeField] public ExposedReference<GameObject> sourceGameObject;

        /// <summary>
        /// Prefab object that will be instantiated.
        /// </summary>
        [SerializeField] public GameObject prefabGameObject;

        /// <summary>
        /// Indicates whether Particle Systems will be controlled.
        /// </summary>
        [SerializeField] public bool updateParticle = true;

        /// <summary>
        /// Random seed to supply particle systems that are set to use autoRandomSeed
        /// </summary>
        /// <remarks>
        /// This is used to maintain determinism when playing back in timeline. Sub emitters will be assigned incrementing random seeds to maintain determinism and distinction.
        /// </remarks>
        [SerializeField] public uint particleRandomSeed;

        /// <summary>
        /// Indicates whether playableDirectors are controlled.
        /// </summary>
        [SerializeField] public bool updateDirector = true;

        /// <summary>
        /// Indicates whether Monobehaviours implementing ITimeControl will be controlled.
        /// </summary>
        [SerializeField] public bool updateITimeControl = true;

        /// <summary>
        /// Indicates whether to search the entire hierarchy for controllable components.
        /// </summary>
        [SerializeField] public bool searchHierarchy = false;

        /// <summary>
        /// Indicate whether GameObject activation is controlled
        /// </summary>
        [SerializeField] public bool active = true;

        /// <summary>
        /// Indicates the active state of the GameObject when Timeline is stopped.
        /// </summary>
        [SerializeField] public ActivationControlPlayable.PostPlaybackState postPlayback = ActivationControlPlayable.PostPlaybackState.Revert;

        PlayableAsset m_ControlDirectorAsset;
        double m_Duration = PlayableBinding.DefaultDuration;
        bool m_SupportLoop;

        private static HashSet<PlayableDirector> s_ProcessedDirectors = new HashSet<PlayableDirector>();
        private static HashSet<GameObject> s_CreatedPrefabs = new HashSet<GameObject>();

        // does the last instance created control directors and/or particles
        internal bool controllingDirectors { get; private set; }
        internal bool controllingParticles { get; private set; }

        // 新增的一些自定义变量

        [SerializeField] private bool isDestroyParticleUntilParticlePlayComplete = true;
        /// <summary>
        /// SourceGameObject是自己传进来的，但是也可以控制是否在轨道结束时，即PlayableBehaviour.OnDestroy()时销毁
        /// </summary>
        [SerializeField] private bool isDestroySourceGameObjectOnDestroy = true;
        [SerializeField] [HideInInspector] public string assetPath;

        private bool isDestroyByEntity = false;

        /// <summary>
        /// Creates the root of a Playable subgraph to control the contents of the game object.
        /// </summary>
        /// <param name="graph">PlayableGraph that will own the playable</param>
        /// <param name="go">The GameObject that triggered the graph build</param>
        /// <returns>The root playable of the subgraph</returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            // case 989856
            if (prefabGameObject != null)
            {
                if (s_CreatedPrefabs.Contains(prefabGameObject))
                {
                    Debug.LogWarningFormat("Control Track Clip ({0}) is causing a prefab to instantiate itself recursively. Aborting further instances.", name);
                    return Playable.Create(graph);
                }
                s_CreatedPrefabs.Add(prefabGameObject);
            }

            Playable root = Playable.Null;
            var playables = new List<Playable>();

            GameObject sourceObject = sourceGameObject.Resolve(graph.GetResolver());
#if UNITY_EDITOR

#endif
            if (prefabGameObject != null)
            {
                Transform parenTransform = sourceObject != null ? sourceObject.transform : null;
                var controlPlayable = CustomPrefabControlPlayable.Create(graph, prefabGameObject, parenTransform, isDestroyParticleUntilParticlePlayComplete);

                sourceObject = controlPlayable.GetBehaviour().prefabInstance;
                playables.Add(controlPlayable);
            }

            m_Duration = PlayableBinding.DefaultDuration;
            m_SupportLoop = false;

            controllingParticles = false;
            controllingDirectors = false;

            if (sourceObject != null)
            {
                if (isDestroySourceGameObjectOnDestroy)
                {
                    // sourceObject也会被自动销毁
                    var controlPlayable = ScriptPlayable<ParticleControlBehaviour>.Create(graph);
                    var particleControlBehaviour = controlPlayable.GetBehaviour();
                    particleControlBehaviour.Init(sourceObject, isDestroyParticleUntilParticlePlayComplete, isDestroyByEntity);
                    playables.Add(controlPlayable);
                }

                var directors = updateDirector ? GetComponent<PlayableDirector>(sourceObject) : k_EmptyDirectorsList;
                var particleSystems = updateParticle ? GetControllableParticleSystems(sourceObject) : k_EmptyParticlesList;

                // update the duration and loop values (used for UI purposes) here
                // so they are tied to the latest gameObject bound
                UpdateDurationAndLoopFlag(directors, particleSystems);

                var director = go.GetComponent<PlayableDirector>();
                if (director != null)
                    m_ControlDirectorAsset = director.playableAsset;

                if (go == sourceObject && prefabGameObject == null)
                {
                    Debug.LogWarningFormat("Control Playable ({0}) is referencing the same PlayableDirector component than the one in which it is playing.", name);
                    active = false;
                    if (!searchHierarchy)
                        updateDirector = false;
                }

                if (active)
                    CreateActivationPlayable(sourceObject, graph, playables);

                if (updateDirector)
                    SearchHierarchyAndConnectDirector(directors, graph, playables, prefabGameObject != null);

                if (updateParticle)
                    SearchHierarchyAndConnectParticleSystem(particleSystems, graph, playables);

                if (updateITimeControl)
                    SearchHierarchyAndConnectControlableScripts(GetControlableScripts(sourceObject), graph, playables);

                // Connect Playables to Generic to Mixer
                root = ConnectPlayablesToMixer(graph, playables);
            }

            if (prefabGameObject != null)
                s_CreatedPrefabs.Remove(prefabGameObject);

            if (!root.IsValid())
                root = Playable.Create(graph);

            return root;
        }

        /// <summary>
        /// This function is called when the object is loaded.
        /// </summary>
        public void OnEnable()
        {
            // can't be set in a constructor
            if (particleRandomSeed == 0)
                particleRandomSeed = (uint)Random.Range(1, k_MaxRandInt);
        }

        /// <summary>
        /// Returns the duration in seconds needed to play the underlying director or particle system exactly once.
        /// </summary>
        public override double duration { get { return m_Duration; } }

        /// <summary>
        /// Returns the capabilities of TimelineClips that contain a ControlPlayableAsset
        /// </summary>
        public ClipCaps clipCaps
        {
            get { return ClipCaps.ClipIn | ClipCaps.SpeedMultiplier | (m_SupportLoop ? ClipCaps.Looping : ClipCaps.None); }
        }



        static Playable ConnectPlayablesToMixer(PlayableGraph graph, List<Playable> playables)
        {
            var mixer = Playable.Create(graph, playables.Count);

            for (int i = 0; i != playables.Count; ++i)
            {
                ConnectMixerAndPlayable(graph, mixer, playables[i], i);
            }

            mixer.SetPropagateSetTime(true);

            return mixer;
        }

        void CreateActivationPlayable(GameObject root, PlayableGraph graph,
            List<Playable> outplayables)
        {
            var activation = ActivationControlPlayable.Create(graph, root, postPlayback);
            if (activation.IsValid())
                outplayables.Add(activation);
        }

        void SearchHierarchyAndConnectParticleSystem(IEnumerable<ParticleSystem> particleSystems, PlayableGraph graph,
            List<Playable> outplayables)
        {
            foreach (var particleSystem in particleSystems)
            {
                if (particleSystem != null)
                {
                    controllingParticles = true;
                    outplayables.Add(ParticleControlPlayable.Create(graph, particleSystem, particleRandomSeed));
                }
            }
        }

        void SearchHierarchyAndConnectDirector(IEnumerable<PlayableDirector> directors, PlayableGraph graph,
            List<Playable> outplayables, bool disableSelfReferences)
        {
            foreach (var director in directors)
            {
                if (director != null)
                {
                    if (director.playableAsset != m_ControlDirectorAsset)
                    {
                        outplayables.Add(DirectorControlPlayable.Create(graph, director));
                        controllingDirectors = true;
                    }
                    // if this self references, disable the director.
                    else if (disableSelfReferences)
                    {
                        director.enabled = false;
                    }
                }
            }
        }

        static void SearchHierarchyAndConnectControlableScripts(IEnumerable<MonoBehaviour> controlableScripts, PlayableGraph graph, List<Playable> outplayables)
        {
            foreach (var script in controlableScripts)
            {
                outplayables.Add(TimeControlPlayable.Create(graph, (ITimeControl)script));
            }
        }

        static void ConnectMixerAndPlayable(PlayableGraph graph, Playable mixer, Playable playable,
            int portIndex)
        {
            graph.Connect(playable, 0, mixer, portIndex);
            mixer.SetInputWeight(playable, 1.0f);
        }

        internal IList<T> GetComponent<T>(GameObject gameObject)
        {
            var components = new List<T>();
            if (gameObject != null)
            {
                if (searchHierarchy)
                {
                    gameObject.GetComponentsInChildren<T>(true, components);
                }
                else
                {
                    gameObject.GetComponents<T>(components);
                }
            }
            return components;
        }

        internal static IEnumerable<MonoBehaviour> GetControlableScripts(GameObject root)
        {
            if (root == null)
                yield break;

            foreach (var script in root.GetComponentsInChildren<MonoBehaviour>())
            {
                if (script is ITimeControl)
                    yield return script;
            }
        }

        internal void UpdateDurationAndLoopFlag(IList<PlayableDirector> directors, IList<ParticleSystem> particleSystems)
        {
            if (directors.Count == 0 && particleSystems.Count == 0)
                return;

            const double invalidDuration = double.NegativeInfinity;

            var maxDuration = invalidDuration;
            var supportsLoop = false;

            foreach (var director in directors)
            {
                if (director.playableAsset != null)
                {
                    var assetDuration = director.playableAsset.duration;

                    //if (director.playableAsset is TimelineAsset && assetDuration > 0.0)
                    //    // Timeline assets report being one tick shorter than they actually are, unless they are empty
                    //    assetDuration = (double)((DiscreteTime)assetDuration).OneTickAfter();

                    maxDuration = Math.Max(maxDuration, assetDuration);
                    supportsLoop = supportsLoop || director.extrapolationMode == DirectorWrapMode.Loop;
                }
            }

            foreach (var particleSystem in particleSystems)
            {
                maxDuration = Math.Max(maxDuration, particleSystem.main.duration);
                supportsLoop = supportsLoop || particleSystem.main.loop;
            }

            m_Duration = double.IsNegativeInfinity(maxDuration) ? PlayableBinding.DefaultDuration : maxDuration;
            m_SupportLoop = supportsLoop;
        }

        IList<ParticleSystem> GetControllableParticleSystems(GameObject go)
        {
            var roots = new List<ParticleSystem>();

            // searchHierarchy will look for particle systems on child objects.
            // once a particle system is found, all child particle systems are controlled with playables
            // unless they are subemitters

            if (searchHierarchy || go.GetComponent<ParticleSystem>() != null)
            {
                GetControllableParticleSystems(go.transform, roots, s_SubEmitterCollector);
                s_SubEmitterCollector.Clear();
            }

            return roots;

        }

        static void GetControllableParticleSystems(Transform t, ICollection<ParticleSystem> roots, HashSet<ParticleSystem> subEmitters)
        {
            var ps = t.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                if (!subEmitters.Contains(ps))
                {
                    roots.Add(ps);
                    CacheSubEmitters(ps, subEmitters);
                }
            }

            for (int i = 0; i < t.childCount; ++i)
            {
                GetControllableParticleSystems(t.GetChild(i), roots, subEmitters);
            }
        }

        static void CacheSubEmitters(ParticleSystem ps, HashSet<ParticleSystem> subEmitters)
        {
            if (ps == null)
                return;

            for (int i = 0; i < ps.subEmitters.subEmittersCount; i++)
            {
                subEmitters.Add(ps.subEmitters.GetSubEmitterSystem(i));
                // don't call this recursively. subEmitters are only simulated one level deep.
            }
        }

        /// <inheritdoc/>
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            // This method is no longer called by Control Tracks.
            if (director == null)
                return;

            // prevent infinite recursion
            if (s_ProcessedDirectors.Contains(director))
                return;
            s_ProcessedDirectors.Add(director);

            var gameObject = sourceGameObject.Resolve(director);
            if (gameObject != null)
            {
                if (updateParticle)// case 1076850 -- drive all emitters, not just roots.
                    PreviewParticles(driver, gameObject.GetComponentsInChildren<ParticleSystem>(true));

                if (active)
                    PreviewActivation(driver, new[] { gameObject });

                if (updateITimeControl)
                    PreviewTimeControl(driver, director, GetControlableScripts(gameObject));

                if (updateDirector)
                    PreviewDirectors(driver, GetComponent<PlayableDirector>(gameObject));
            }
            s_ProcessedDirectors.Remove(director);
        }

        internal static void PreviewParticles(IPropertyCollector driver, IEnumerable<ParticleSystem> particles)
        {
            foreach (var ps in particles)
            {
                driver.AddFromName<ParticleSystem>(ps.gameObject, "randomSeed");
                driver.AddFromName<ParticleSystem>(ps.gameObject, "autoRandomSeed");
            }
        }

        internal static void PreviewActivation(IPropertyCollector driver, IEnumerable<GameObject> objects)
        {
            foreach (var gameObject in objects)
                driver.AddFromName(gameObject, "m_IsActive");
        }

        internal static void PreviewTimeControl(IPropertyCollector driver, PlayableDirector director, IEnumerable<MonoBehaviour> scripts)
        {
            foreach (var script in scripts)
            {
                var propertyPreview = script as IPropertyPreview;
                if (propertyPreview != null)
                    propertyPreview.GatherProperties(director, driver);
                else
                    driver.AddFromComponent(script.gameObject, script);
            }
        }

        internal static void PreviewDirectors(IPropertyCollector driver, IEnumerable<PlayableDirector> directors)
        {
            foreach (var childDirector in directors)
            {
                if (childDirector == null)
                    continue;

                var timeline = childDirector.playableAsset as TimelineAsset;
                if (timeline == null)
                    continue;

                timeline.GatherProperties(childDirector, driver);
            }
        }

        public void SetIsDestroyByEntity(bool isDestroyByEntity) { this.isDestroyByEntity = isDestroyByEntity; }

    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ParticleControlClip))]
    public class ParticleControlClipEditor : UnityEditor.Editor
    {
        private bool isValid = false;

        private void OnEnable()
        {
            var property = this.serializedObject.FindProperty($"{nameof(ParticleControlClip.assetPath)}");
            isValid = UnityEditor.AssetDatabase.AssetPathToGUID(property.stringValue) != "";
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var property = this.serializedObject.FindProperty($"{nameof(ParticleControlClip.assetPath)}");
            var prevColor = GUI.color;
            GUI.color = isValid ? Color.green : Color.red;
            UnityEditor.EditorGUILayout.LabelField($"资源路径：{property.stringValue} {(isValid ? "合法" : "非法")}");
            GUI.color = prevColor;
        }
    }
#endif

}

```

##### ParticleControlBehaviour

``` csharp
using BordlessFramework.PluginExtension;
using IQIGame.Onigao.Game;
using System;

namespace UnityEngine.Timeline
{
    [Serializable]
    [NotKeyable]
    public class ParticleControlBehaviour : CustomPlayableBehaviour
    {
        GameObject sourceGameObject;
        bool isDestroyParticleUntilParticlePlayComplete;
        bool isDestroyByEntity;

        public void Init(GameObject sourceGameObject, bool isDestroyParticleUntilParticlePlayComplete, bool isDestroyByEntity)
        {
            this.sourceGameObject = sourceGameObject;
            this.isDestroyParticleUntilParticlePlayComplete = isDestroyParticleUntilParticlePlayComplete;
            this.isDestroyByEntity = isDestroyByEntity;
        }

        protected override void OnDestroy()
        {
            if (!isDestroyParticleUntilParticlePlayComplete)
            {
                if (isDestroyByEntity) GameEntry.Entity.HideEntity(sourceGameObject.GetComponent<EntityBase>());
                else Object.Destroy(sourceGameObject);
            }
            else
            {
                var mono = sourceGameObject.AddComponent<BattleAutoDestroyParticle>();
                mono.Init(isDestroyByEntity: this.isDestroyByEntity);
            }
        }
    }
}

```

