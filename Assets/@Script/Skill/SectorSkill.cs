using Cysharp.Threading.Tasks;
using Mono.Cecil;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SectorSkill : Skill
{
    public override void UseSkill(SkillData data)
    {
        attack = data.Attack;
        MoveProjectile(data.TargetPos , data.Angle * 2).Forget();
    }
    async UniTaskVoid MoveProjectile(Vector3 targetPos, float angleOffset)
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        float distance = Vector2.Distance(transform.position, targetPos);
        float dist = Vector2.Distance(Owner.transform.position, transform.position);
        float scale = angleOffset / 360 * 2 * Mathf.PI * dist;

        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.localScale = new Vector3(scale * transform.localScale.x, scale * transform.localScale.y, scale * transform.localScale.z);
        while (distance >= 0.1f)
        {
            transform.position += dir * Time.deltaTime * Speed;

            dist = Vector2.Distance(Owner.transform.position, transform.position);
            scale = angleOffset / 360 * 2 * Mathf.PI * dist;
            distance = Vector2.Distance(transform.position, targetPos);
            transform.localScale = new Vector3(scale * 0.2f, scale * 0.2f, scale * 0.2f);
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
