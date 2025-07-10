using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RectangleScope : Scope
{
    //private void Start()
    //{
    //    MeshData meshData = new MeshData();
    //    meshData.Size = 1f;
    //    GenerateMesh(meshData);
    //    SetMeshActive(true);
    //}
    public override void GenerateMesh(MeshData data)
    {
        base.GenerateMesh(data);
        mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(new Vector3(0, -0.1f));
        vertices.Add(new Vector3(0, 0.1f));
        vertices.Add(new Vector3(data.aRange/2, 0.1f));
        vertices.Add(new Vector3(data.aRange/2, -0.1f));

        List<int> triangles = new List<int>()
        {
            0, 1, 2,
            2, 3, 0
        };
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        List<Vector2> points = new List<Vector2>();

        foreach(Vector3 v in vertices)
        {
            points.Add(v);
        }
        polygonCollider.points = points.ToArray();
        meshRenderer.material = ScopeMaterial;
    }
    public override void LookAt(Transform target)
    {
        Debug.Log("스코프 조준");
        base.LookAt(target);

    }
}
