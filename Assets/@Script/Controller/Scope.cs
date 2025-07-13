using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Material ScopeMaterial;

    protected MeshRenderer meshRenderer;
    protected MeshFilter meshFilter;
    protected PolygonCollider2D polygonCollider;
    public HeroController Owner;
    public Mesh mesh;

    public virtual void GenerateMesh(MeshData data)
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer.sortingOrder = 5;
    }
    public void SetMeshActive(bool isActive)
    {
        if (meshRenderer)
        if (isActive)
        {
            meshFilter.mesh = mesh;
        }
        else
        {
            meshFilter.mesh = null;
        }
    }
    public virtual void LookAt(Transform target)
    {
        if (target == null)
            return;
        transform.position = Owner.transform.position;
        Vector2 destPos = target.position - transform.position;
        float angle = Mathf.Atan2(destPos.y, destPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public virtual List<MonsterController> GetTargets()
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

