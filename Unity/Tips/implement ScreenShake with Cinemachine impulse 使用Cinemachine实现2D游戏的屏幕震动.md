
- 新建Noise Settings

  <img alt="EditorSetting.png" src="assets/new noise settings.png" width="500" height="" >
- 设置Noise Raw Signal

  注意，这里只需要设置Position Y的Amplitude为1（另外建议勾选non-random，使得震动是非随机的），而Position X的 Amplitude 设置为0，否则方向是不对的

  <img alt="EditorSetting.png" src="assets/set amplitude and direction.png" width="500" height="" >
- 用以下代码产生震动信号
  ``` csharp
  /// <summary>
  /// 屏幕震动
  /// </summary>
  /// <param name="direction">方向</param>
  /// <param name="strength">强度</param>
      public static void ShakeScreen(Vector2 direction, float strength)
      {
          var ImpulseSource = GetComponent<CinemachineImpulseSource>();
          ImpulseSource.GenerateImpulse(direction * strength);
      }
  }

  ```

### 参考
- [unity cinemachine package api](https://docs.unity3d.com/Packages/com.unity.cinemachine@2.2/api/Cinemachine.CinemachineImpulseSource.html)
- [unity cinemachine package manual](https://docs.unity3d.com/Packages/com.unity.cinemachine@2.3/manual/CinemachineImpulseSourceOverview.html)
