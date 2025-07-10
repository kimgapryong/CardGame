using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RectangleSkill : Skill
{
    public override void UseSkill(SkillData data)
    {
        attack = data.Attack;
        StartCoroutine(MoveProjectile(data.TargetPos));
    }
    IEnumerator MoveProjectile(Vector3 targetPos)
    {
        float distance = Vector2.Distance(targetPos, transform.position);
        Vector3 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        while (distance >= 0.1f)
        {
            transform.position += dir * Time.deltaTime * Speed;
            distance = Vector2.Distance(targetPos, transform.position);
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monsterController = collision.GetComponent<MonsterController>();
        if (monsterController == null)
            return;
        Debug.Log($"{collision.name}에게 {attack} 공격력 입혔음");
        monsterController.OnDamage(Owner, attack);
    }
}
