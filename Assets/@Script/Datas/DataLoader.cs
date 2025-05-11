using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[Serializable]
public class HeroData
{
    public int HeroID;
    public string HeroName;
    public string ElementType;
    public HeroType ArangeType;
    public List<LevelData> LevelData = new List<LevelData>();
}
[Serializable]
public class LevelData
{
    public int Level;
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
public class HeroLoader : ILoader<int, HeroData>
{
    List<HeroData> data = new List<HeroData> ();
    public Dictionary<int, HeroData> MakeDic()
    {
        Dictionary<int, HeroData> heroDic = new Dictionary<int, HeroData>();
        foreach(var hero in data)
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

public class SkillLoader : ILoader<int, Skills>
{
    List<Skills> data = new List<Skills>();
    public Dictionary<int, Skills> MakeDic()
    {
        Dictionary<int, Skills> skillDic = new Dictionary<int, Skills>();
        foreach(var skill in data)
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
public class AnimationLoader : ILoader<int, AnimationData>
{
    List<AnimationData> data = new List<AnimationData>();
    public Dictionary<int, AnimationData> MakeDic()
    {
        Dictionary<int, AnimationData> animDic = new Dictionary<int, AnimationData>();
        foreach (var animation in data)
            animDic.Add(animation.AnimationID, animation);

        return animDic;
    }

    public bool Validate()
    {
        return true;
    }
}
