using UnityEngine;

public class Projectile : Skill
{
    public override void UseSkill(SkillData data)
    {
        target = data.TargetTransform;
        attack = data.Attack;
    }
    private void Update()
    {
        if (target == null || Owner == null)
            Destroy(gameObject);
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.position += dir * Time.deltaTime * Speed;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monsterController = collision.GetComponent<MonsterController>();
        if (collision.transform != target)
            return;
        if (monsterController == null)
            return;

        monsterController.OnDamage(Owner, attack);
        Destroy(gameObject);
    }
}
