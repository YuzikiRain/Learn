- Doesn't work
```csharp
Vector3 viewPos = Camera.main.WorldToScreenPoint(anchorPoint.position);
GetComponent<RectTransform>().position = new Vector2(viewPos.x, viewPos.y);
```

- works only when Component<CanvasScaler>().UIScaleMode set to Constant Pixel Size or ScreenPixel Equals Component<CanvasScaler>().ReferenceResolution
```csharp
Vector3 viewPos = Camera.main.WorldToScreenPoint(anchorPoint.position);
GetComponent<RectTransform>().anchoredPosition = new Vector2(viewPos.x, viewPos.y);
```

- works only when Component<CanvasScaler>().UIScaleMode set to Scale With Screen Size
```csharp
Vector3 viewPos = Camera.main.WorldToScreenPoint(anchorPoint.position);
var referenceResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
var widthRatio = referenceResolution.x / Camera.main.scaledPixelWidth;
var heightRatio = referenceResolution.y / Camera.main.scaledPixelHeight;
GetComponent<RectTransform>().anchoredPosition = new Vector2(viewPos.x * widthRatio, viewPos.y * heightRatio);
```

- Works no matter what Component<CanvasScaler>().UIScaleMode is set
```csharp
Vector3 viewPos = Camera.main.WorldToViewportPoint(anchorPoint.position);

GetComponent<RectTransform>().anchorMin = new Vector2(viewPos.x, viewPos.y);
GetComponent<RectTransform>().anchorMax = new Vector2(viewPos.x, viewPos.y);
```

### Reference
[answers.unity.com/questions by Eyevi](https://answers.unity.com/questions/1188717/worldtoscreenpoint-is-giving-me-a-wrong-position.html)
