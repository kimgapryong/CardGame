using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[Serializable]
public class HeroData
{
    public int HeroID;
    public string ElementType;
    [SerializeField] private string ArangeType;
    [SerializeField] private string HeroRating;

    public HeroType Arange_Type => ParseEnumOrDefault(ArangeType, Define.HeroType.Close);
    public HeroRating Hero_Rating => ParseEnumOrDefault(HeroRating, Define.HeroRating.Common);

    private T ParseEnumOrDefault<T>(string value, T defaultValue) where T : struct
    {
        if (Enum.TryParse<T>(value, ignoreCase: true, out var result))
            return result;
        return defaultValue;
    }

    public List<LevelData> LevelData = new List<LevelData>();
}
[Serializable]
public class LevelData
{
    public string HeroName;
    public int Level;
    public string Sprite;
    public string HeroInduce;
    public List<HeroLevelData> HeroLevelData = new List<HeroLevelData>();
    public List<SkillMapping> SkillMapData = new List<SkillMapping>();

}
[Serializable]
public class HeroLevelData
{
    public float Attack;
    public float Arange;
    public float AtkSpeed;
    public float Upgrade;
}
[Serializable]
public class SkillMapping
{
    public int SkillID;
    public int AnimationID;
}

[Serializable]
public class HeroLoader : ILoader<int, HeroData>
{
    public List<HeroData> heroes = new List<HeroData> ();
    public Dictionary<int, HeroData> MakeDic()
    {
        Dictionary<int, HeroData> heroDic = new Dictionary<int, HeroData>();
        foreach(var hero in heroes)
            heroDic.Add(hero.HeroID, hero);
            

        return heroDic;
    }

    public bool Validate()
    {
        return true;
    }
}

[Serializable]
public class Skills
{
    public int SkillID;
    public string SkillPre;
}

[Serializable]
public class SkillLoader : ILoader<int, Skills>
{
    public List<Skills> skills = new List<Skills>();
    public Dictionary<int, Skills> MakeDic()
    {
        Dictionary<int, Skills> skillDic = new Dictionary<int, Skills>();
        foreach(var skill in skills)
            skillDic.Add(skill.SkillID, skill);

        return skillDic;
    }

    public bool Validate()
    {
        return true;
    }
}

[Serializable]
public class AnimationData
{
    public int AnimationID;
    public string AnimationPre;
}
[Serializable]
public class AnimationLoader : ILoader<int, AnimationData>
{
    public List<AnimationData> anims = new List<AnimationData>();
    public Dictionary<int, AnimationData> MakeDic()
    {
        Dictionary<int, AnimationData> animDic = new Dictionary<int, AnimationData>();
        foreach (var animation in anims)
            animDic.Add(animation.AnimationID, animation);

        return animDic;
    }

    public bool Validate()
    {
        return true;
    }
}
