using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Material ScopeMaterial;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    PolygonCollider2D polygonCollider;

    Mesh mesh;
    public void GenerateMesh(float aRange, float offsetAngle)
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();

        Vector2 destPos = Vector2.right * aRange / 2;

        float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;
        float destPosLength = destPos.magnitude;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        vertices.Add(Vector3.zero);
        normals.Add(new Vector3(0, 0, 1));

        for (float i = angle - offsetAngle; i <= angle + offsetAngle; i += 0.5f)
        {
            Vector3 pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i) * destPosLength, Mathf.Sin(Mathf.Deg2Rad * i) * destPosLength, 0);
            normals.Add(new Vector3(0, 0, 1));
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
    public void SetMeshActive(bool isActive)
    {
        if (isActive)
        {
            meshFilter.mesh = mesh;
        }
        else
        {
            meshFilter.mesh = null;
        }
    }
    public void LookAt(Transform target)
    {
        if (target == null)
            return;
        Vector2 destPos = target.position - transform.position;
        float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    public List<MonsterController> GetTargets()
    {
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.NoFilter();

        List<Collider2D> colliders = new List<Collider2D>();
        polygonCollider.Overlap(filter2D, colliders);

        List<MonsterController> targets = new List<MonsterController>();
        
        foreach (Collider2D collider in colliders)
        {
            MonsterController mController = collider.GetComponent<MonsterController>();
            if (mController == null)
                continue;
            targets.Add(mController);
        }
        return targets;
    }

}
