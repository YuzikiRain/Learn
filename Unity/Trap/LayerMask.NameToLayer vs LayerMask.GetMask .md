- LayerMask.NameToLayer return the layer index, which is the same as the index in inspector.
- LayerMask.GetMask("LayerA", "LayerB", "LayerC", ...) return 1 << indexOfLayerA | 1 << indexOfLayerB | 1 << indexOfLayerC...

### Reference
- https://docs.unity3d.com/ScriptReference/LayerMask.GetMask.html
- https://docs.unity3d.com/ScriptReference/LayerMask.NameToLayer.html