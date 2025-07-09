using System.Collections.Generic;
using UnityEngine;

public class CircleScope : Scope
{
    private void Start()
    {
        MeshData meshData = new MeshData();
        meshData.Size = 1f;
        GenerateMesh(meshData);
        SetMeshActive(true);
    }
    public override void GenerateMesh(MeshData data)
    {
        base.GenerateMesh(data);

        mesh = new Mesh();

        Vector2 destPos = Vector2.right;

        float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;
        float length = data.Size / 2;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        vertices.Add(Vector3.zero);

        for (float i = 0; i <= 360; i += 0.5f)
        {
            Vector3 pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(i * Mathf.Deg2Rad), 0);
            pos *= length;
            vertices.Add(pos);
        }
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);

        List<int> indices = new List<int>();

        for (int i = 0; i < vertices.Count; i++)
        {
            if (i >= vertices.Count - 1)
                break;
            indices.Add(0);
            indices.Add(i);
            indices.Add(i + 1);
        }

        Vector2[] points = new Vector2[vertices.Count];
        for (int i = 0;i < points.Length; i++)
        {
            points[i] = vertices[i];
        }
        polygonCollider.points = points;
        mesh.SetTriangles(indices, 0);
        meshRenderer.material = ScopeMaterial;
    }
    public override void LookAt(Transform target)
    {
        transform.position = target.position;
    }
}
