### TextBackground

控制背景如何生成

放到任意UI下，删除其他组件仅保留`CanvasRenderer`

``` csharp
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BordlessFramework.UI
{
    public class TextBackground : Graphic
    {
        private static Vector4 uvLeftBottom = new Vector4(0f, 0f, 0f, 0f);
        private static Vector4 uvLeftTop = new Vector4(0f, 1f, 0f, 0f);
        private static Vector4 uvRightBottom = new Vector4(1f, 0f, 0f, 0f);
        private static Vector4 uvRightTop = new Vector4(1f, 1f, 0f, 0f);

        /*[SerializeField] [Range(0f, 1f)] */
        private float progress = 0.5f;
        //[SerializeField] private float deltaX;
        //[SerializeField] private float deltaY;
        [SerializeField] private float scaleX;
        [SerializeField] private float scaleY;

        [SerializeField] private TextBackgroundSource backgroundSource;

        public void SetProgress(float progress) { this.progress = progress; }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            var vertices = backgroundSource.GetVertices();
            // 在Text的Mesh的基础上生成对应的Mesh
            var newVertices = ModifyVertices(vertices);
            toFill.AddUIVertexTriangleStream(newVertices);
        }

        const int interval = 6;
        private List<UIVertex> ModifyVertices(List<UIVertex> vertices)
        {
            int characterCount = vertices.Count / interval;
            // 生成新的顶点，不影响顶点源数据
            List<UIVertex> newVertices = new List<UIVertex>(vertices.Capacity);

            // 取得每个字符的中心和宽高
            List<Vector3> vertexCenters = new List<Vector3>(characterCount);
            List<Vector2> characterSizes = new List<Vector2>(characterCount);
            for (int i = 0; i < vertices.Count; i += interval)
            {
                var position0 = vertices[i].position;
                var position2 = vertices[i + 2].position;
                Vector3 center = new Vector3(
                  (position0.x + position2.x) * 0.5f,
                  (position0.y + position2.y) * 0.5f,
                  (position0.z + position2.z) * 0.5f
                    );
                vertexCenters.Add(center);
                characterSizes.Add(new Vector2((position2.x - position0.x) * 0.5f, -(position2.y - position0.y) * 0.5f));
            }

            for (int i = 0; i < vertices.Count; i++)
            {
                int whichVertex = i % interval;
                int vertexToCharacterIndex = i / interval;
                var newVertex = vertices[i];
                var size = characterSizes[vertexToCharacterIndex];
                // 设置uv，刚好采样整个texture
                switch (whichVertex)
                {
                    case 0:
                        newVertex.position += new Vector3(-size.x * scaleX, size.y * scaleY, 0f);
                        newVertex.uv0 = uvLeftTop;
                        break;
                    case 1:
                        newVertex.position += new Vector3(size.x * scaleX, size.y * scaleY, 0f);
                        newVertex.uv0 = uvRightTop;
                        break;
                    case 2:
                        newVertex.position += new Vector3(size.x * scaleX, -size.y * scaleY, 0f);
                        newVertex.uv0 = uvRightBottom;
                        break;
                    case 3:
                        newVertex.position += new Vector3(size.x * scaleX, -size.y * scaleY, 0f);
                        newVertex.uv0 = uvRightBottom;
                        break;
                    case 4:
                        newVertex.position += new Vector3(-size.x * scaleX, -size.y * scaleY, 0f);
                        newVertex.uv0 = uvLeftBottom;
                        break;
                    case 5:
                        newVertex.position += new Vector3(-size.x * scaleX, size.y * scaleY, 0f);
                        newVertex.uv0 = uvLeftTop;
                        break;
                    default:
                        break;
                }

                var progressPerCharacter = 1f / characterCount;
                // 反推progress处于哪个顶点，这里仅修改顶点色的alpha，rgb仍可通过Text组件的color属性控制
                int progressToCharacterIndex = (int)(progress / progressPerCharacter);
                // 在progress之前的显示
                if (vertexToCharacterIndex < progressToCharacterIndex) newVertex.color.a = 255;
                // 之后的隐藏
                else if (vertexToCharacterIndex > progressToCharacterIndex) newVertex.color.a = 0;
                // 中间的过渡
                else
                {
                    var t = ((progress % progressPerCharacter) / progressPerCharacter);
                    newVertex.color.a = (byte)Mathf.Lerp(0, 255, t);
                }
                // 使用Graphic.color.alpha值控制整体alpha
                newVertex.color.a = (byte)(newVertex.color.a * color.a);
                newVertices.Add(newVertex);
            }

            return newVertices;
        }
    }
}
```

### TextBackgroundSource

当Text组件变化时，重新生成对应背景mesh

放到Text同一物体下

``` csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BordlessFramework.UI
{
    public class TextBackgroundSource : BaseMeshEffect
    {
        [SerializeField] private Text text;
        [SerializeField] private TextBackground textBackground;
        [SerializeField] [Range(0f, 1f)] private float progress = 0.5f;
        private List<UIVertex> vertices;

        protected override void OnEnable()
        {
            text.RegisterDirtyVerticesCallback(UpdateProgress);
        }

        protected override void OnDisable()
        {
            text.UnregisterDirtyVerticesCallback(UpdateProgress);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            text = GetComponent<Text>();

            SetProgress(this.progress);
        }
#endif

        /// <summary>
        /// 设置文本显示总进度，范围[0,1]
        /// </summary>
        /// <param name="progress"></param>
        public void SetProgress(float progress)
        {
            this.progress = progress;

            text.SetVerticesDirty();
        }

        public void UpdateProgress()
        {
            textBackground.SetProgress(this.progress);
            textBackground.SetVerticesDirty();
        }

        public override void ModifyMesh(VertexHelper vertexHelper)
        {
            vertices = ListPool<UIVertex>.Get();
            // 仅取得Text组件的顶点，用于传递给TextBackground使用
            vertexHelper.GetUIVertexStream(vertices);

            ModifyVertices();
            vertexHelper.Clear();
            vertexHelper.AddUIVertexTriangleStream(vertices);
        }

        public List<UIVertex> GetVertices()
        {
            return vertices;
        }

        const int interval = 6;
        private void ModifyVertices()
        {
            int characterCount = vertices.Count / interval;

            for (int i = 0; i < vertices.Count; i++)
            {
                int whichCharacter = i / interval;
                var newVertex = vertices[i];

                var progressPerCharacter = 1f / characterCount;
                int progressToCharacterIndex = (int)(progress / progressPerCharacter);
                if (whichCharacter < progressToCharacterIndex) newVertex.color.a = 255;
                else if (whichCharacter > progressToCharacterIndex) newVertex.color.a = 0;
                else
                {
                    var t = ((progress % progressPerCharacter) / progressPerCharacter);
                    newVertex.color.a = (byte)Mathf.Lerp(0, 255, t);
                }
                vertices[i] = newVertex;
            }
        }
    }
}
```

