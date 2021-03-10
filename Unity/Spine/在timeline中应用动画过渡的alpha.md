找到 ```SpineAnimationStateMixerBehaviour```

``` csharp
// NOTE: This function is called at runtime and edit time. Keep that in mind when setting the values of properties.
public override void ProcessFrame (Playable playable, FrameData info, object playerData) {
    ...
    if (clipData.animationReference == null) {
        float mixDuration = clipData.customDuration ? clipData.mixDuration : state.Data.DefaultMix;
        state.SetEmptyAnimation(trackIndex, mixDuration);
    } else {
        if (clipData.animationReference.Animation != null) {
            Spine.TrackEntry trackEntry = state.SetAnimation(trackIndex, clipData.animationReference.Animation, clipData.loop);

            trackEntry.EventThreshold = clipData.eventThreshold;
            trackEntry.DrawOrderThreshold = clipData.drawOrderThreshold;
            trackEntry.TrackTime = (float)inputPlayable.GetTime() * (float)inputPlayable.GetSpeed();
            trackEntry.TimeScale = (float)inputPlayable.GetSpeed();
            trackEntry.AttachmentThreshold = clipData.attachmentThreshold;
            // ******apply alpha here for runtime*****
            trackEntry.Alpha = clipData.alpha;
            if (clipData.customDuration)
                trackEntry.MixDuration = clipData.mixDuration;
        }
        //else Debug.LogWarningFormat("Animation named '{0}' not found", clipData.animationName);
    }
    ...
}

public void PreviewEditModePose(Playable playable, ISkeletonComponent skeletonComponent, IAnimationStateComponent animationStateComponent, SkeletonAnimation skeletonAnimation, SkeletonGraphic skeletonGraphic) {
    ...
    // Apply Pose
    dummyAnimationState.Update(0);
    dummyAnimationState.Apply(skeleton);
} else {
    if (toAnimation != null)
        // ******apply alpha here for preview in editor*****
        toAnimation.Apply(skeleton, 0, toClipTime, clipData.loop, null, clipData.alpha, MixBlend.Setup, MixDirection.In);
    	//toAnimation.Apply(skeleton, 0, toClipTime, clipData.loop, null, 1f, MixBlend.Setup, MixDirection.In);
}
    ...
}
```

