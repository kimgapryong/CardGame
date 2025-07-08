using UnityEngine;

public class Projectile : Skill
{
    public override void UseSkill(Transform targetTransform, float attack, float distance = 0)
    {
        target = targetTransform;
        this.attack = attack;
    }
    private void Update()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.position += dir * Time.deltaTime * Speed;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monsterController = GetComponent<MonsterController>();

        if (collision.transform != target)
            return;
        if (monsterController == null)
            return;

        monsterController.OnDamage(Owner, attack);
        Destroy(gameObject);
    }
}
