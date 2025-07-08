using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectorScope : Scope
{
    public override void GenerateMesh(MeshData data)
    {
        base.GenerateMesh(data);
        SectorMeshData meshData = data as SectorMeshData;
        mesh = new Mesh();

        Vector2 destPos = Vector2.right;

        float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;
        float length = meshData.aRange / 2;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        vertices.Add(Vector3.zero);

        for (float i = angle - meshData.angle; i <= angle + meshData.angle; i += 0.5f)
        {
            Vector3 pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i), 0);
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

        mesh.SetTriangles(indices, 0);
        meshRenderer.material = ScopeMaterial;

        Vector2[] colliderPoints = new Vector2[vertices.Count];
        for (int i = 0; i < colliderPoints.Count(); i++)
            colliderPoints[i] = vertices[i];

        polygonCollider.points = colliderPoints;
    }
}
