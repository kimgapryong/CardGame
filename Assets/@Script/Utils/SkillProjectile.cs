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
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null) return;

        if (collision.transform == target)
        {
            MonsterController monster = collision.GetComponent<MonsterController>();
            if (monster != null)
            {
                monster.OnDamage(null, damage);
            }

            Destroy(gameObject);
        }
    }
}
