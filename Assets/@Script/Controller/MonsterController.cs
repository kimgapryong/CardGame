using System;
using System.Collections;
using UnityEngine;

public class MonsterController : BaseController
{
    
    public Action<float, float> hpAction;
    MonsterData _monsterData;
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

   
}
