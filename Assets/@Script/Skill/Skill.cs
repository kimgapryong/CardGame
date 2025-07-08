using UnityEngine;

public class Skill : MonoBehaviour
{
    public BaseController Owner;
    protected Transform target;

    protected float attack;
    public float Speed;
    public virtual void UseSkill(Transform targetTransform, float attack, float distance = 0)
    {

    }
    public virtual void UseSkill(Vector2 targetPos, float attack)
    {

    }
}
