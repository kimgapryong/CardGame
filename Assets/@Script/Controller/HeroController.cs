using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using static UnityEngine.GraphicsBuffer;

public class HeroController : CretureController
{
    public bool SetTile { get; set; } = false;
    private bool isLoaded = false;
    
    public int curLevel { get; private set; } = 0;

    public HeroData _heroData { get; private set; }

    private GameObject skillPre;
    private Transform argTrans;
    private Collider2D coll;

    private AtkArange atkArg;
    
    private MonsterController curTarget;
    private Tile _tile;
    Scope scope;

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
        SetScope();
        return true;
    }
    async void SetScope()
    {
        GameObject go = null;
        MeshData meshData = new MeshData();
        switch (_heroData.LevelData[curLevel].Atk_Arange)
        {
            case Define.AtkArange.Single:
                break;
            case Define.AtkArange.Aoe:
                break;
            case Define.AtkArange.Rectangle:
                go = await Manager.Resource.Instantiate("Rectangle-Scope", null);

                scope = go.GetComponent<Scope>();
                scope.transform.position = transform.position;
                scope.Owner = this;

                meshData.aRange = _heroData.LevelData[curLevel].HeroLevelData.Arange;
                break;
            case Define.AtkArange.Sector:
                go = await Manager.Resource.Instantiate("Sector-Scope", null);

                scope = go.GetComponent<Scope>();
                scope.transform.position = transform.position;
                scope.Owner = this;

                meshData.angle = _heroData.LevelData[curLevel].AngleOffset;
                meshData.aRange = _heroData.LevelData[curLevel].HeroLevelData.Arange;
                break;
            case Define.AtkArange.CoreCross:
                go = await Manager.Resource.Instantiate("CoreCross-Scope", null);
                scope = go.GetComponent<Scope>();
                scope.transform.position = transform.position;
                scope.Owner = this;

                meshData.aRange = _heroData.LevelData[curLevel].HeroLevelData.Arange;
                break;
            case Define.AtkArange.Circle:
                go = await Manager.Resource.Instantiate("Circle-Scope", null);

                scope = go.GetComponent<Scope>();
                scope.transform.position = transform.position;
                scope.Owner = this;

                meshData.angle = _heroData.LevelData[curLevel].AngleOffset;
                meshData.aRange = _heroData.LevelData[curLevel].HeroLevelData.Arange;
                break;
        }
        if (scope == null)
            return;
        scope.GenerateMesh(meshData);
        //scope.SetMeshActive(atkArg.gameObject.activeSelf);
    }
    private void Update()
    {
        if (!isLoaded || !SetTile)
            return;

        UpdateMethod();
    }
    public void SetTileCell(Tile tile)
    {
        _tile = tile;
        tile.hero = gameObject;
        SetTile = true;
    }
    protected override void UpdateMethod()
    {
        
        switch (State)
        {
            case Define.State.Idle:
                TryAttack();
                break;

            case Define.State.Attack:
                break; // 코루틴으로 처리되므로 여기선 대기
        }
        if (scope == null)
            return;
        if (curTarget == null)
            return;
        
        scope.LookAt(curTarget.transform);
    }
    public void UpgradeLevel()
    {
        if (curLevel + 1 > _heroData.LevelData.Count - 1)
            return;

        curLevel++;

        Manager.Resource.Instantiate(_heroData.LevelData[curLevel].HeroSprite, callback:(obj) =>
        {
            HeroController hc = obj.GetOrAddComponent<HeroController>();
            hc.CurLevelUp(curLevel);
            hc.SetTileCell(_tile);
            hc.SetInfo(_heroData);
            obj.transform.position = transform.position;

            Manager.UI.CloseAllPopupUI();
            Manager.UI.ShowPopupUI<Upgrade_Pop>(callback: (pop) =>
            {
                pop.SetInfo(hc._heroData, hc, hc._tile);
            });
            if (scope != null)
                Destroy(scope.gameObject);
            Destroy(gameObject);
        });
    }
    public void CurLevelUp(int level)
    {
        curLevel= level;
    }
    public void OffArg()
    {
        argTrans.gameObject.SetActive(false);

        if (scope == null)
            return;
        scope.SetMeshActive(false);
    }
    public void OnArg()
    {
        argTrans.gameObject.SetActive(true);

        if (scope == null)
            return;
        scope.SetMeshActive(true);
    }
    protected override void TryAttack()
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
        StartCoroutine(CoAttack(curTarget));
    }

    private IEnumerator CoAttack(MonsterController target)
    {
        Debug.LogWarning(target);
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
                AttackProjectile(target);
                break;
            case Define.AtkArange.Aoe:
                AoeAttack();
                break;
            case Define.AtkArange.Sector:
                AttackSector(target);
                break;
            case Define.AtkArange.Rectangle:
                AttackRectangle(target);
                break;
            case Define.AtkArange.CoreCross:
                AttackCoreCross();
                break;
            case Define.AtkArange.Circle:
                AttackCircle(target);
                break;
        }
        Skills skillData = Manager.Data.SkillDatas[_heroData.HeroID];
        Manager.Sound.PlaySFX(skillData.Sound);
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
    private void AttackProjectile(MonsterController target)
    {
        if (target == null)
            return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        go.transform.position = transform.position;
        Debug.LogWarning(go);
        int cardLevel = Manager.Game.CardDataDict[_heroData.HeroID].level;
        float attack = _heroData.LevelData[curLevel].HeroLevelData.Attack + Manager.Data.HeroUpgradeDatas[_heroData.HeroID].AttackIncreaseAmount * cardLevel + _heroData.BaseAttack;
        Skills skills = Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID];

        Skill skill = go.GetComponent<Skill>();
        SkillData skillData = new SkillData();
        skillData.Attack = attack;
        skillData.TargetTransform = target.transform;
        skill.UseSkill(skillData);
    }
    private void AttackCoreCross()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 dir = new Vector2(Mathf.Cos(i * 45 * Mathf.Deg2Rad), Mathf.Sin(i * 45 * Mathf.Deg2Rad));
            float distance = _heroData.LevelData[curLevel].HeroLevelData.Arange;

            GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
            go.transform.position = transform.position;
            Debug.LogWarning(go);
            int cardLevel = Manager.Game.CardDataDict[_heroData.HeroID].level;
            float attack = _heroData.LevelData[curLevel].HeroLevelData.Attack + Manager.Data.HeroUpgradeDatas[_heroData.HeroID].AttackIncreaseAmount * cardLevel + _heroData.BaseAttack;
            Skills skills = Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID];

            Skill skill = go.GetComponent<Skill>();
            SkillData skillData = new SkillData();
            skillData.Attack = attack;
            skillData.TargetPos = transform.position + dir * distance/2;

            skill.UseSkill(skillData);
        }
    }
    void AttackRectangle(MonsterController target)
    {
        if (target == null)
            return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        go.transform.position = transform.position;
        Debug.LogWarning(go);
        int cardLevel = Manager.Game.CardDataDict[_heroData.HeroID].level;
        float attack = _heroData.LevelData[curLevel].HeroLevelData.Attack + Manager.Data.HeroUpgradeDatas[_heroData.HeroID].AttackIncreaseAmount * cardLevel + _heroData.BaseAttack;
        Skills skills = Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID];
        Skill skill = go.GetComponent<Skill>();

        SkillData skillData = new SkillData();
        Vector3 dir = (target.transform.position - transform.position).normalized;
        skillData.TargetPos = transform.position + dir * (_heroData.LevelData[curLevel].HeroLevelData.Arange / 2);
        skillData.Attack = attack;

        skill.UseSkill(skillData);
    }

    void AttackSector(MonsterController target)
    {
        if (target == null)
            return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        go.transform.position = transform.position;
        Debug.LogWarning(go);
        int cardLevel = Manager.Game.CardDataDict[_heroData.HeroID].level;
        float attack = _heroData.LevelData[curLevel].HeroLevelData.Attack + Manager.Data.HeroUpgradeDatas[_heroData.HeroID].AttackIncreaseAmount * cardLevel + _heroData.BaseAttack;
        Skills skills = Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID];
        Skill skill = go.GetComponent<Skill>();
        
        SkillData skillData = new SkillData();
        Vector3 dir = (target.transform.position - transform.position).normalized;
        skillData.TargetPos = transform.position + dir * _heroData.LevelData[curLevel].HeroLevelData.Arange / 2;
        skillData.Angle = _heroData.LevelData[curLevel].AngleOffset;
        skillData.Attack =  attack;

        skill.Owner = this;
        skill.UseSkill(skillData);
    }
    void AttackCircle(MonsterController target)
    {
        if (target == null)
            return;

        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);
        go.transform.position = transform.position;
        Debug.LogWarning(go);
        int cardLevel = Manager.Game.CardDataDict[_heroData.HeroID].level;
        float attack = _heroData.LevelData[curLevel].HeroLevelData.Attack + Manager.Data.HeroUpgradeDatas[_heroData.HeroID].AttackIncreaseAmount * cardLevel + _heroData.BaseAttack;
        Skills skills = Manager.Data.SkillDatas[_heroData.LevelData[curLevel].SkillMapData.SkillID];
        Skill skill = go.GetComponent<Skill>();

        SkillData skillData = new SkillData();
        skillData.Attack = attack;
        skillData.TargetPos = target.transform.position;

        skill.Owner = this;
        skill.UseSkill(skillData);
    }
    private void AoeAttack()
    {
        if (atkArg.targets.Count <= 0)
            return;
        int cardLevel = Manager.Game.CardDataDict[_heroData.HeroID].level;
        float attack = _heroData.LevelData[curLevel].HeroLevelData.Attack + Manager.Data.HeroUpgradeDatas[_heroData.HeroID].AttackIncreaseAmount * cardLevel + _heroData.BaseAttack;
        GameObject go = Object.Instantiate(skillPre, transform.position, Quaternion.identity);

        if (_heroData.LevelData[curLevel].AngleOffset == 0)
        {
            for (int i = 0; i < atkArg.targets.Count; i++)
            {
                var monster = atkArg.targets[i];
                if (monster != null)
                    monster.OnDamage(this, attack);
            }
        }
        else
        {
            
            List<MonsterController> targetMonsters = scope.GetTargets();
            AoeProjectile proj = go.GetComponent<AoeProjectile>();
            proj.SetTarget(this, _heroData.LevelData[curLevel].HeroLevelData.Arange, (curTarget.transform.position - transform.position).normalized);
            foreach (var targetMonster in targetMonsters)
            {
                targetMonster.OnDamage(this, attack);
            }
        }
    }
    public void SetInfo(HeroData data)
    {
        _heroData = data;

        float normalScale = 1.0f / transform.localScale.x; //히어로 크기 정규화식
        transform.Find("Arange").localScale = Vector3.one * _heroData.LevelData[curLevel].HeroLevelData.Arange * normalScale;
        transform.Find("AtkArange").localScale = Vector3.one * _heroData.LevelData[curLevel].HeroLevelData.Arange * normalScale;
        transform.Find("AtkArange").GetOrAddComponent<AtkArange>();

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
