using System;
using System.Collections.Generic;
using UnityEngine;

public class CoreCrossScope : Scope
{
    public override void GenerateMesh(MeshData data)
    {
        base.GenerateMesh(data);
        CombineInstance[] combine = new CombineInstance[4];
        for (int i = 0; i < 4; i++)
        {
            combine[i] = new CombineInstance();
            combine[i].transform = Matrix4x4.identity;
            float angle = i * 45;
            
            Mesh _mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();

            Vector3 y = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 x = new Vector3(Mathf.Cos((angle + 90) * Mathf.Deg2Rad), Mathf.Sin((angle + 90) * Mathf.Deg2Rad)) * .1f;

            vertices.Add((-y - x) * data.Size);
            vertices.Add((y - x) * data.Size);
            vertices.Add((y + x) * data.Size);
            vertices.Add((-y + x) * data.Size);

            _mesh.SetVertices(vertices);
            List<int> triangles = new List<int>()
            {
                0, 1, 2,
                2, 3, 0
            };
            _mesh.triangles = triangles.ToArray();

            combine[i].mesh = _mesh;
        }
        this.mesh = new Mesh();
        this.mesh.CombineMeshes(combine);
        meshRenderer.material = ScopeMaterial;

        Vector2[] points = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < points.Length; i++)
            points[i] = mesh.vertices[i];
        polygonCollider.points = points;
    }
    public override void LookAt(Transform target)
    {
        transform.position = Owner.transform.position;
        transform.Rotate(0,0,90 * Time.deltaTime);
    }
    public override List<MonsterController> GetTargets()
    {
        //반환할 타겟들
        List<MonsterController> target = new List<MonsterController>();

        //콜라이더 안에 있는 모든 객체들(Collider2D 컴포넌트를 가진 얘들만)
        List<Collider2D> overlapedColliders = new List<Collider2D>();
        //콜라이더 안에 있는 모든 객체를 위 변수에 넣어줌
        polygonCollider.Overlap(overlapedColliders);

        //monsterController있는지 확인함.
        foreach(Collider2D collider in overlapedColliders)
        {
            MonsterController monsterController = collider.GetComponent<MonsterController>();
            if (monsterController == null)
                continue;
            target.Add(monsterController);
        }

        return target;
    }
}
