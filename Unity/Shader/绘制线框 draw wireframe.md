## 底层API GL.Vertex

绘制顺序在Camera的最后，无法控制

``` c#
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 绘制线框组件
/// 使用方式：1.添加MeshFilter组件，然后附加对应mesh 2.右键该组件选择”根据MeshFilter组件生成Line数据“
/// </summary>
[ExecuteInEditMode]
public class MeshWireframe : MonoBehaviour
{
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private string lineColorProperty = "_LineColor";
    [HideInInspector] [SerializeField] private List<Mesh> meshs;
    [HideInInspector] [SerializeField] private List<Vector3> vertices;

    private void OnRenderObject()
    {
        if (vertices == null || vertices.Count == 0 || lineMaterial == null || meshs == null || meshs.Count == 0) return;

        // 必须每帧都调用
        lineMaterial.SetColor(lineColorProperty, lineColor);
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        //绘制线框用的模型矩阵就是该物体的（即线框的transform和本物体相同）
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.LINES);
        for (int i = 0; i < vertices.Count / 3; i++)
        {
            GL.Vertex(vertices[i * 3]);
            GL.Vertex(vertices[i * 3 + 1]);
            GL.Vertex(vertices[i * 3 + 1]);
            GL.Vertex(vertices[i * 3 + 2]);
            GL.Vertex(vertices[i * 3 + 2]);
            GL.Vertex(vertices[i * 3]);
        }
        GL.End();

        GL.PopMatrix();
    }

    private void GenerateLines()
    {
        if (vertices != null)
            vertices.Clear();
        else
            vertices = new List<Vector3>();
        foreach (var mesh in meshs)
        {
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length / 3; i++)
            {
                this.vertices.Add(vertices[triangles[i * 3]]);
                this.vertices.Add(vertices[triangles[i * 3 + 1]]);
                this.vertices.Add(vertices[triangles[i * 3 + 2]]);
            }
        }
    }

#if UNITY_EDITOR

    [ContextMenu("根据MeshFilter组件生成顶点数据")]
    private void GetMeshesDataFromMeshFilter()
    {
        if (meshs != null) meshs.Clear();
        else meshs = new List<Mesh>();

        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>(true);
        if (meshFilters != null && meshFilters.Length > 0)
        {
            foreach (var meshFilter in meshFilters) meshs.Add(meshFilter.sharedMesh);
            GenerateLines();
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        if (skinnedMeshRenderers != null && skinnedMeshRenderers.Length > 0)
        {
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers) meshs.Add(skinnedMeshRenderer.sharedMesh);
            GenerateLines();
        }
    }
#endif
}
```

参考：[Unity中绘制线框(Wireframe)的几种方法 - 简书 (jianshu.com)](https://www.jianshu.com/p/e95e6507659c)