using System.Collections.Generic;
using UnityEngine;

public class Skill_12 : Skill
{
    public Material SkillMat;
    public override void UseSkill(Vector3 position)
    {
        Mesh mesh = new Mesh();

        Vector2 destPos = position - gameObject.transform.position;

        float angle = Mathf.Atan2(destPos.y, destPos.x);
        float destPosLength = destPos.magnitude;

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add((Vector2)transform.position);
        for (float i = angle - 15f; i <= angle + 15f; i += 0.1f)
        {
            Vector3 pos = new Vector3(Mathf.Cos(i) * destPosLength, Mathf.Sin(i) * destPosLength);
            vertices.Add(pos);
            vertices.Add(transform.position);
        }
        mesh.SetVertices(vertices);

        int[] triangles = new int[vertices.Count];

        for (int i = 0; i < triangles.Length; i++)
            triangles[i] = i;

        mesh.triangles = triangles;

        gameObject.AddComponent<MeshCollider>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mController= gameObject.GetComponent<MonsterController>();
        if (mController == null)
            return;
        mController.OnDamage(Owner, Damage);
    }
}
