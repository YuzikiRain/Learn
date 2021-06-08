对于每一个从InputActionMap里得到的InputAction，都要调用 InputAction.Enable() 来激活 InputAction，因为默认是disable的

```csharp
InputActionAsset inputActionAsset = Object.Instantiate(GameAssetManager.LoadInputAction("Input/PlayerInput.inputactions"));
// 必须调用 InputAction.Enable() 来激活 InputAction，因为默认是disable的
InputActionMap inputActionMap = inputActionAsset.actionMaps[0];
// 可以不调用 InputActionMap.Enable() 来激活 InputActionMap，因为默认是enable的
// inputActionMap.Enable();
Move = inputActionMap.FindAction("Move");
Move.Enable();
Jump = inputActionMap.FindAction("Jump");
Jump.Enable();
```

