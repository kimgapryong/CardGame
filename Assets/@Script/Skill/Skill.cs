using UnityEngine;

public class Skill : MonoBehaviour
{
    public BaseController Owner;

    protected HeroLevelData heroLevelData;
    public virtual void UseSkill(Transform targetTransform, HeroLevelData heroLevelData)
    {

    }

}
