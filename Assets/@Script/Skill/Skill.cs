using UnityEngine;

public class Skill : MonoBehaviour
{
    public BaseController Owner;
    protected Transform target;

    protected float attack;
    public float Speed;
    public virtual void UseSkill(SkillData data)
    {

    }
}
