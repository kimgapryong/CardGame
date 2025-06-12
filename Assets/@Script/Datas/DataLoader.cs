using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using static Define;

[Serializable]
public class HeroData
{
    public int HeroID;
    public string ElementType;
    [SerializeField] private string ArangeType;
    [SerializeField] private string HeroRating;
    [SerializeField] private string HeroAbility;
    public HeroType Arange_Type => ParseEnumOrDefault(ArangeType, Define.HeroType.Close);
    public HeroRating Hero_Rating => ParseEnumOrDefault(HeroRating, Define.HeroRating.Common);
    public HeroAbility Hero_Ability => ParseEnumOrDefault(HeroAbility, Define.HeroAbility.Atkker);

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
    public string HeroSprite;
    
    public HeroLevelData HeroLevelData;
    public SkillMapping SkillMapData;
    public float BaseAttack;

    [SerializeField] private string AtkArange;  
    public Define.AtkArange Atk_Arange => ParseEnumOrDefault(AtkArange, Define.AtkArange.Single);

    private T ParseEnumOrDefault<T>(string value, T defaultValue) where T : struct
    {
        if (Enum.TryParse<T>(value, true, out var result))
            return result;
        return defaultValue;
    }

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
        {
            heroDic.Add(hero.HeroID, hero);
        }
            

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

[Serializable]
public class MonsterData
{
    public int MonsterID;
    public string MonsterName;
    public float Speed;
    public float Hp;
    public float Money;
}
[Serializable]
public class MonsterLoader : ILoader<int, MonsterData>
{
    public List<MonsterData> monsters = new List<MonsterData>();
    public Dictionary<int, MonsterData> MakeDic()
    {
        Dictionary<int, MonsterData> monDic = new Dictionary<int, MonsterData>();
        foreach (var mon in monsters)
            monDic.Add(mon.MonsterID, mon);

        return monDic;
    }

    public bool Validate()
    {
        return true;
    }
}
[Serializable]
public class ProductData
{
    public int ProductID;
    public string ProductName;
    public string Type;
    public string ProductSpriteKey;
    public int Price;
    public int CardID;
    public int GainGoods;
    public int Qnt;
    public string Pay;
    public Define.ProductType ProductType => ParseEnumOrDefault<Define.ProductType>(Type, Define.ProductType.Card);
    public Define.PayType PayType => ParseEnumOrDefault<Define.PayType>(Pay, Define.PayType.Gold);

    private T ParseEnumOrDefault<T>(string value, T defaultValue) where T : struct
    {
        if (Enum.TryParse<T>(value, true, out var result))
            return result;
        return defaultValue;
    }
}
[Serializable]
public class ProductLoader : ILoader<int, ProductData>
{
    public List<ProductData> products = new List<ProductData>();
    public Dictionary<int, ProductData> MakeDic()
    {
        Dictionary<int, ProductData> dict = new Dictionary<int, ProductData>();

        for (int i = 0; i  < products.Count; i++)
        {
            dict.Add(products[i].ProductID, products[i]);
        }
        return dict;
    }

    public bool Validate()
    {
        return true;
    }
}
[Serializable]
public class UpgradeData
{
    public int HeroID;
    public List<UpgradeValue> Levels = new List<UpgradeValue>();
}

[Serializable]
public class UpgradeValue
{
    public int RequiredCardNumber;
    public int Price;
    public float Attack;
}

[Serializable]
public class UpgradeDataLoader : ILoader<int, UpgradeData>
{
    public List<UpgradeData> upgradeDatas = new List<UpgradeData>();
    public Dictionary<int, UpgradeData> MakeDic()
    {
        Dictionary<int, UpgradeData> dict = new Dictionary<int, UpgradeData>();

        foreach(UpgradeData upgrade in upgradeDatas)
        {
            dict.Add(upgrade.HeroID, upgrade);
        }
        return dict;
    }

    public bool Validate()
    {
        return true;
    }
}