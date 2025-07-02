using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skill_12 : Skill
{
    public Material SkillMat;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    //테스트 용도 객체 나중에 주석 처리
    //public Transform testTarget;
    private void Start()
    {

    }
    //스킬 발사하는 거
    //특정 방향에서 각도 오프셋 15도 정도 줘서 원형으로 되게 함.
    public override void UseSkill(Transform targetTransform, float attack, float aRange)
    {
        this.attack= attack;
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector2 destPos = (targetTransform.position - transform.position).normalized * aRange;

        float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;
        float destPosLength = destPos.magnitude;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();

        vertices.Add(transform.position);
        normals.Add(new Vector3(0, 0, 1));

        for (float i = angle - 15f; i <= angle + 15f; i += 0.5f)
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

        meshFilter.mesh = mesh;
        meshRenderer.material = SkillMat;
        //transform.Rotate(180, 0, 0);

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = mesh;

        StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1f);
        //Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mController = gameObject.GetComponent<MonsterController>();
        if (mController == null)
            return;
        mController.OnDamage(Owner, attack);
    }
}
