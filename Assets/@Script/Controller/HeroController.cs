using System.Collections;
using UnityEngine;

public class HeroController : BaseController
{
    public bool SetTile { get; set; } = false;
    private bool isLoaded = false;
    private bool isAttacking = false;
    private int curLevel = 0;

    private HeroData _heroData;
    private GameObject skillPre;
    private AtkArange atkArg;
    private Define.State _state;
    private MonsterController curTarget;

    public Define.State State
    {
        get => _state;
        set => _state = value;
    }

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        StartCoroutine(CoWaitForSkill());

        atkArg = transform.Find("AtkArange").GetComponent<AtkArange>();
        State = Define.State.Idle;

        return true;
    }

    private void Update()
    {
        if (!isLoaded || !SetTile)
            return;

        UpdateMethod();
    }

    protected virtual void UpdateMethod()
    {
        switch (State)
        {
            case Define.State.Idle:
                TryAttack();
                break;

            case Define.State.Attack:
                break; // 코루틴으로 처리되므로 여기선 대기
        }
    }

    private void TryAttack()
    {
        if (atkArg.targets.Count == 0 || isAttacking)
            return;

        curTarget = atkArg.targets[0];
        if (curTarget != null)
        {
            StartCoroutine(CoAttack(curTarget));
        }
    }

    private IEnumerator CoAttack(MonsterController target)
    {
        State = Define.State.Attack;
        isAttacking = true;

        if (target == null || !atkArg.targets.Contains(target))
        {
            isAttacking = false;
            State = Define.State.Idle;
            yield break;
        }

        Attack(target);

        float delay = _heroData.LevelData[curLevel].HeroLevelData.AtkSpeed;
        yield return new WaitForSeconds(delay);

        isAttacking = false;
        State = Define.State.Idle;
    }

    private void Attack(MonsterController target)
    {
        if (target == null) return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        go.GetOrAddComponent<SkillProjectile>().SetTarget(target.transform, _heroData.LevelData[curLevel].HeroLevelData.Attack);
    }

    public void SetInfo(HeroData data)
    {
        _heroData = data;
        Manager.Resource.LoadAsync<GameObject>(
            Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID].SkillPre,
            (obj) => { skillPre = obj; }
        );
    }

    private IEnumerator CoWaitForSkill()
    {
        while (skillPre == null)
            yield return null;

        isLoaded = true;
    }
}
