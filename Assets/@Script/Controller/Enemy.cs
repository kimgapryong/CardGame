using UnityEngine;

public class Enemy : MonoBehaviour
{

    MonsterData _monsterData;
    public void SetInfo(MonsterData data, float Hp)
    {
        _monsterData = data;
        _maxHp = Hp;
        _speed = data.Speed;
    }

    private float _speed;
    public float Speed { get { return _speed; } private set { _speed = value; } }

    private float _maxHp;
    public float MaxHp { get { return _maxHp; } private set { _maxHp = value; } }

    private float _curHp;
    public float CurHp { get { return _curHp; } set { _curHp = value; } }


}

