using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterController : CretureController
{
    
    public Action<float, float> hpAction;
    MonsterData _monsterData;

    private Vector3 direct;
    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        
        State = Define.State.Move;
        return true;
    }
  
    public void SetInfo(MonsterData data, float Hp)
    {
        _monsterData = data;
        _maxHp = Hp;
        _curHp = Hp;
    }
    private float _maxHp;
    public float MaxHp { get { return _maxHp; } private set { _maxHp = value; } }

    private float _curHp;
    public float CurHp {
        get { return _curHp; }
        set
        {
            hpAction?.Invoke(value, _maxHp);
            _curHp = value;
        }
    }
    public virtual void OnDamage(BaseController controller, float damage)
    {
        CurHp -= damage;
        if(CurHp <= 0 )
            OnDie();
    }

    protected virtual void OnDie()
    {
        Manager.Time.Money += _monsterData.Money;
        Destroy(gameObject);
    }
    protected override void TryMove()
    {
        if (direct.x * transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

    }

    public void GetNormalizePos(Vector3 curPos, Vector3 nextPos)
    {
        
        Vector3 normalizePos = (nextPos - curPos).normalized;
        Debug.Log(normalizePos);
        direct = normalizePos;
    }

}
