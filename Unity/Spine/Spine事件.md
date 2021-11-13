



![img](http://esotericsoftware.com/img/spine-runtimes-guide/spine-unity/callbackchart.png)

### 监听事件

``` csharp
SkeletonAnimation skeletonAnimation;
Spine.AnimationState animationState;

void Awake () {
   skeletonAnimation = GetComponent<SkeletonAnimation>();
   animationState = skeletonAnimation.AnimationState;

   // registering for events raised by any animation
   animationState.Start += OnSpineAnimationStart;
   animationState.Interrupt += OnSpineAnimationInterrupt;
   animationState.End += OnSpineAnimationEnd;
   animationState.Dispose += OnSpineAnimationDispose;
   animationState.Complete += OnSpineAnimationComplete;

   animationState.Event += OnUserDefinedEvent;

   // registering for events raised by a single animation track entry
   Spine.TrackEntry trackEntry = animationState.SetAnimation(trackIndex, "walk", true);
   trackEntry.Start += OnSpineAnimationStart;
   trackEntry.Interrupt += OnSpineAnimationInterrupt;
   trackEntry.End += OnSpineAnimationEnd;
   trackEntry.Dispose += OnSpineAnimationDispose;
   trackEntry.Complete += OnSpineAnimationComplete;
   trackEntry.Event += OnUserDefinedEvent;
}

public void OnSpineAnimationStart(TrackEntry trackEntry) {
   // Add your implementation code here to react to start events
}
public void OnSpineAnimationInterrupt(TrackEntry trackEntry) {
   // Add your implementation code here to react to interrupt events
}
public void OnSpineAnimationEnd(TrackEntry trackEntry) {
   // Add your implementation code here to react to end events
}
public void OnSpineAnimationDispose(TrackEntry trackEntry) {
   // Add your implementation code here to react to dispose events
}
public void OnSpineAnimationComplete(TrackEntry trackEntry) {
   // Add your implementation code here to react to complete events
}


string targetEventName = "targetEvent";
string targetEventNameInFolder = "eventFolderName/targetEvent";

public void OnUserDefinedEvent(Spine.TrackEntry trackEntry, Spine.Event e) {

   if (e.Data.Name == targetEventName) {
      // Add your implementation code here to react to user defined event
   }
}

// you can cache event data to save the string comparison
Spine.EventData targetEventData;
void Start () {
   targetEventData = skeletonAnimation.Skeleton.Data.FindEvent(targetEventName);
}
public void OnUserDefinedEvent(Spine.TrackEntry trackEntry, Spine.Event e) {

   if (e.Data == targetEventData) {
      // Add your implementation code here to react to user defined event
   }
}
```

### spine协程

``` csharp
The following yield instructions are provided:

WaitForSpineAnimation. Waits until a Spine.TrackEntry raises one of the specified events.

C# var track = skeletonAnimation.state.SetAnimation(0, "interruptible", false);
 var completeOrEnd = WaitForSpineAnimation.AnimationEventTypes.Complete |
                      WaitForSpineAnimation.AnimationEventTypes.End;
 yield return new WaitForSpineAnimation(track, completeOrEnd);
WaitForSpineAnimationComplete. Waits until a Spine.TrackEntry raises a Complete event.

C# var track = skeletonAnimation.state.SetAnimation(0, "talk", false);
 yield return new WaitForSpineAnimationComplete(track);
WaitForSpineAnimationEnd. Waits until a Spine.TrackEntry raises an End event.

C# var track = skeletonAnimation.state.SetAnimation(0, "talk", false);
 yield return new WaitForSpineAnimationEnd(track);
WaitForSpineEvent. Waits until a Spine.AnimationState raises a user-defined Spine.Event (named in Spine editor).

C# yield return new WaitForSpineEvent(skeletonAnimation.state, "spawn bullet");
 // You can also pass a Spine.Event's Spine.EventData reference.
 Spine.EventData spawnBulletEvent; // cached in e.g. Start()
 ..
 yield return new WaitForSpineEvent(skeletonAnimation.state, spawnBulletEvent);
```



### 参考

http://esotericsoftware.com/spine-unity-events