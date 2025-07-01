using UnityEngine;

public class Skill : MonoBehaviour
{
    public BaseController Owner;

    protected float attack;
    public virtual void UseSkill(Transform targetTransform, float attack, float aRange)
    {

    }

}
