using Cysharp.Threading.Tasks;
using UnityEngine;

public class CoreCrossSkill : Skill
{
    public override void UseSkill(SkillData data)
    {
        attack = data.Attack;
        MoveProjectile(data.TargetPos).Forget();
    }
    async UniTaskVoid MoveProjectile(Vector3 targetPos)
    {
        transform.position = targetPos + new Vector3(-10, -20);
        Vector3 dir = (targetPos - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        while (Vector2.Distance(transform.position, targetPos) <= 0.1f)
        {
            transform.position += dir * Time.deltaTime * Speed;
            await UniTask.Yield();
        }

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monsterController = collision.GetComponent<MonsterController>();
        if (monsterController == null)
            return;

        monsterController.OnDamage(Owner, attack);
    }
}
