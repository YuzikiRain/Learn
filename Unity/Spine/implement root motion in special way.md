
### Environment
Unity2018.4.1f, Spine runtime for unity 3.7

### Steps

- open Spine/Runtime/spine-csharp/Bone.cs, and modify like below
```csharp
/// <summary>Computes the world transform using the parent bone and the specified local transform.</summary>
public void UpdateWorldTransform (float x, float y, float rotation, float scaleX, float scaleY, float shearX, float shearY) {
  ax = x;
  ay = y;
  arotation = rotation;
  ascaleX = scaleX;
  ascaleY = scaleY;
  ashearX = shearX;
  ashearY = shearY;
  appliedValid = true;
  Skeleton skeleton = this.skeleton;

  Bone parent = this.parent;
  if (parent == null) { // Root bone.
    float rotationY = rotation + 90 + shearY, sx = skeleton.scaleX, sy = skeleton.scaleY;
    a = MathUtils.CosDeg(rotation + shearX) * scaleX * sx;
    b = MathUtils.CosDeg(rotationY) * scaleY * sy;
    c = MathUtils.SinDeg(rotation + shearX) * scaleX * sx;
    d = MathUtils.SinDeg(rotationY) * scaleY * sy;
    worldX = 0f;
    worldY = 0f;
    //worldX = x * sx + skeleton.x;
    //worldY = y * sy + skeleton.y;
    return;
  }
```

- be sure skeletonData is auto generate with .atlas.txt, .png and **.json**(not .skel.bytes)

- drag the skeletonData to Hierarchy and choose "SkeletonMecanim", then a "Unity Animator Controller" will be created where skeletonData is in, and a gameObject named "Spine Mecanim GameObject (dog)" is created in Hierarchy

<img alt="Sprite.png" src="assets/drag to create animator controller.png" width="500" height="" >

- select the skeletonData, click the gear icon and choose "Skeleton Baking"

  <img alt="Sprite.png" src="assets/skeleton bake icon.png" width="1900" height="" >

- in the bake window, check "Bake Animations", and click "Bake Skeleton" button

<img alt="Sprite.png" src="assets/skeleton bake panel.png" width="500" height="" >

- a new folder named "Baked" will be created, which contains a prefab and a "Unity Animator Controller" **(if your skeletonData is generate by .skel.bytes but .json, the animation baked will lose the event info and Position info, and you can't continue the steps later)**

- open animation window, then select animation named "Attack" in animator controller **in baked folder**, use your mouse to select all frames in bone "root", then copy by ctrl + c

<img alt="Sprite.png" src="assets/copy all frames in baked animaton.png" width="1900" height="" >

- back to parent folder named "Dog2", double click animator controller to open animator window, drag animation "Attack" to any layer

<img alt="Sprite.png" src="assets/drag to add animation in animator.png" width="1900" height="" >

- select gameObject named "Spine Mecanim GameObject (dog)" in Hierarchy, in animation window you will see animation "Attack" is list(if you finish previous step successfully), and select it.

<img alt="Sprite.png" src="assets/choose animation Attack.png" width="1900" height="" >

- click "Add Property" button and choose Transform Position

<img alt="Sprite.png" src="assets/add property transform position.png" width="1900" height="" >

-  use your mouse to select all frames in Position, then paste by ctrl + v

<img alt="Sprite.png" src="assets/paste all bone root frames.png" width="1900" height="" >

- don't forget to save this animation by ctrl + s !

- to enable root motion, you should check apply root motion in component Animator of gameObject "Spine Mecanim GameObject (dog)"(if unchecked, hint "Root position or rotation are controlled by curves" will appear below Apply Root Motion in component Animator, and you can't modify position or rotation of this gameObject)

<img alt="Sprite.png" src="assets/check apply root motion.png" width="1900" height="" >

- enjoy it!

### By the way
- if you translate an animation which move bone root every frame(and move 5 units in x axis totally in these frames) with another animation which stay in zero by 0.2 second, the 5 units root movement in transition(0.2 second) will blend to 0 units, so a 5 units root movement animation translate to 0 unit root movement animation result less than 5 units root movement.
**由于Unity Mecanim的动画过渡机制，本应共计在x轴上位移5单位的动画转移到原地不动的动画后，最终造成的位移会小于5**
- if you want to avoid this situation, you can set Transition Duration to 0, or make the animation with root movement not move in last a few frames to fit any other animation.
**为避免这种情况，可以将所有Transition Duration设为0，即不采用自动过渡而是自己提供动画间的过渡动画，另一种方案是每个带有根骨骼运动的动画都在最后几帧不移动，用于与其他任何动画过渡**
