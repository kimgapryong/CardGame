using Cysharp.Threading.Tasks;
using UnityEngine;

public class RectangleSkill : Skill
{
    public override void UseSkill(SkillData data)
    {
        attack = data.Attack;
        MoveProjectile((data.TargetPos - transform.position).normalized).Forget();
    }
    async UniTaskVoid MoveProjectile(Vector3 dir)
    {
        float distance = 0;
        while (distance >= 0.1f)
        {
            transform.position += dir * Time.deltaTime * Speed;
            await UniTask.Yield();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monsterController = collision.GetComponent<MonsterController>();
        if (monsterController == null)
            return;

        monsterController.OnDamage(Owner, attack);
    }
}
