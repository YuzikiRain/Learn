### 名词解释

#### Timeline

-   每个Timeline实例下有若干个Track

#### 轨道Track

-   Timeline窗口下看到的每一行就是轨道

-   TrackBindingType标签指定了受Track影响的类型（一般是继承了MonoBehaviour的组件）

-   TrackClipType标签指定了Track内可创建哪一种Clip类型的实例

-   每个轨道实例内可创建多个（同一类型的）Clip实例

-   ```cshar
    using UnityEngine;
    using UnityEngine.Timeline;
    
    [TrackBindingType(typeof(ControlledComponent))]
    [TrackClipType(typeof(DialogueControlClip))]
    public class DialogueControlTrack : TrackAsset
    {
    }
    ```

#### 剪辑Clip（继承自Playable）

-   本身并不包含任何数据，而是引用了PlayableBehaviour实例作为模板

-   运行时会返回模板的拷贝，因此不会修改模板本身

-   ```csharp
    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    using System;
    
    [Serializable]
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

#### 基类Playable

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

    

