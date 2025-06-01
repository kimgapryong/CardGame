using System.Collections;
using UnityEngine;

public class HeroController : BaseController
{
    public bool SetTile { get; set; } = false;
    private bool isLoaded = false;
    private bool isAttacking = false;
    public int curLevel { get; private set; } = 0;

    public HeroData _heroData { get; private set; }

    private GameObject skillPre;
    private Transform argTrans;
    private Collider2D coll;

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

        argTrans = transform.Find("Arange");
        coll = transform.Find("AtkArange").GetComponent<Collider2D>();
        atkArg = transform.Find("AtkArange").GetComponent<AtkArange>();
        State = Define.State.Idle;

        if (_heroData != null && _heroData.Hero_Ability == Define.HeroAbility.Money)
        {
            coll.enabled = false;
        }

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
    public void UpgradeLevel()
    {
        if (curLevel + 1 > _heroData.LevelData.Count - 1)
            return;

        curLevel++;

        Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[curLevel].HeroSprite, (sprite) =>
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        });
        Manager.Resource.LoadAsync<GameObject>(Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID].SkillPre, (obj) =>
        {
            skillPre = obj;
        });

        float curSize = _heroData.LevelData[curLevel].HeroLevelData.Arange;
        argTrans.localScale = new Vector2(curSize, curSize);
        transform.Find("AtkArange").localScale = new Vector2(curSize, curSize);


    }
    public void OffArg()
    {
        argTrans.gameObject.SetActive(false);
    }
    public void OnArg()
    {
        argTrans.gameObject.SetActive(true);
    }
    private void TryAttack()
    {
        if (isAttacking)
            return;

        if (_heroData.Hero_Ability == Define.HeroAbility.Money)
        {
            StartCoroutine(CoMoneyTick());
            return;
        }

        if (atkArg.targets.Count == 0)
            return;

        curTarget = atkArg.targets[0];
        Debug.Log(curTarget);
        StartCoroutine(CoAttack(curTarget));
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

        switch (_heroData.LevelData[curLevel].Atk_Arange)
        {
            case Define.AtkArange.Single:
                Attack(target);
                break;
            case Define.AtkArange.Aoe:
                AoeAttack();
                break;
            default:
                Attack(target);
                break;
        }
        

        float delay = _heroData.LevelData[curLevel].HeroLevelData.AtkSpeed;
        yield return new WaitForSeconds(delay);

        isAttacking = false;
        State = Define.State.Idle;
    }
    private IEnumerator CoMoneyTick()
    {
        isAttacking = true;
        State = Define.State.Attack;

        yield return new WaitForSeconds(10f);

        while (true)
        {
            // 돈 생성
            Manager.Resource.Instantiate("MoneyParticle", transform);
            Manager.Time.Money += _heroData.LevelData[curLevel].HeroLevelData.Attack;

            float delay = _heroData.LevelData[curLevel].HeroLevelData.AtkSpeed;
            yield return new WaitForSeconds(delay);
        }
    }
    private void Attack(MonsterController target)
    {
        
        if (target == null) return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        go.GetOrAddComponent<SkillProjectile>().SetTarget(target.transform, _heroData.LevelData[curLevel].HeroLevelData.Attack);
    }

    private void AoeAttack()
    {
        if (atkArg.targets.Count <= 0)
            return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        for (int i = 0; i < atkArg.targets.Count; i++)
        {
            var monster = atkArg.targets[i];
            if (monster != null)
                monster.OnDamage(this, _heroData.LevelData[curLevel].HeroLevelData.Attack);
        }
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
