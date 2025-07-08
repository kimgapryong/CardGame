using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private Transform target;
    private float damage;

    public float speed = 5f;

    public void SetTarget(Transform t, float dmg)
    {
        target = t;
        damage = dmg;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (target == null) return;
        Debug.Log("데미지1");
        if (collision.transform != target)
        {
            Debug.Log($"{collision.isTrigger}");
            return;
        }
        Debug.Log("데미지2");
        MonsterController monster = collision.GetComponent<MonsterController>();
        if (monster != null)
            monster.OnDamage(null, damage);
        Debug.Log("데미지3");
        Destroy(gameObject);
    }
}
