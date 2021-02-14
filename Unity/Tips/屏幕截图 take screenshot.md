### ScreenCapture.CaptureScreenshotAsTexture

``` csharp
yield return new WaitForEndOfFrame();
var screenShotTexture = ScreenCapture.CaptureScreenshotAsTexture();
// 相比CaptureScreenshot，可以使用自定义路径保存图片
byte[] bytes = screenShotTexture.EncodeToPNG();
var filePath = $"Assets/test.png";
File.WriteAllBytes(filePath, bytes);
```

### RenderTexture

-   获得RenderTexture

    -   方式一

        ```csharp
        var renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 32, GraphicsFormat.R8G8B8A8_UNorm);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
        ```

    -   方式二

        ``` csharp
        var screenRenderTexture = new RenderTexture(width, height, 0, GraphicsFormat.R8G8B8A8_UNorm);
            var originTargetTexture = camera.targetTexture;
            camera.targetTexture = screenRenderTexture;
            camera.Render();
            camera.targetTexture = originTargetTexture;
        ```

        

-   异步取得渲染数据：```UnityEngine.Rendering.AsyncGPUReadback.Request``` 传入RenderTexture、TextureFormat。**注意这里的 TextureFormat 和 申请RenderTexture的 GraphicsFormat 的数据格式要一致，比如TextureFormat是 TextureFormat.RGBA32，GraphicsFormat 是 GraphicsFormat.R8G8B8A8_UNorm**

    ``` csharp
    private GraphicsFormat _graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
    private TextureFormat _textureFormat = TextureFormat.RGBA32;
    
    public void TakeScreenShot(Camera camera, string path)
    {
        // 设置camera.targetTexture为screenRenderTexture来将当前颜色缓冲输出到RenderTexture上，之后再重置相机设置
        var screenRenderTexture = new RenderTexture(width, height, 0, _graphicsFormat);
        var originTargetTexture = camera.targetTexture;
        camera.targetTexture = screenRenderTexture;
        camera.Render();
        camera.targetTexture = originTargetTexture;
    
        UnityEngine.Rendering.AsyncGPUReadback.Request(screenRenderTexture, 0, _textureFormat, OnCompleteReadback);
    }
    
    void OnCompleteReadback(UnityEngine.Rendering.AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }
    
        byte[] colors = request.GetData<byte>().ToArray();
        // 异步写入方法，不阻塞主线程，ImageConversion.EncodeArrayToPNG是线程安全的所以不会报 must run in main thread 的错误
        Task.Run(() => File.WriteAllBytes(_filePath, ImageConversion.EncodeArrayToPNG(colors, _graphicsFormat, width, height)));
    }
    ```


此方法截图是在其他线程异步写入，不阻塞主线程



参考：https://stackoverflow.com/questions/62864092/unity-extracting-camera-pixel-array-is-incredibly-slow

