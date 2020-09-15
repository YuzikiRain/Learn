### 步骤

-   创建PlayableAsset模板（用于创建Playable的拷贝）

    ```csharp
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

-   创建PlayableBehaviour（设置受控制的组件的字段等来进行表现）

    ```csharp
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

    

-   创建轨道资源脚本

    ```csharp
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Timeline;
    
    [TrackBindingType(typeof(ControlledComponent))]
    [TrackClipType(typeof(DialogueControlClip))]
    public class DialogueControlTrack : TrackAsset
    {
    
    }
    
    ```

-   