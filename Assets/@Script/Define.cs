

using Newtonsoft.Json;
using UnityEngine;

public class Define
{
   public enum HeroRating
    {
        Common,
        Normal,
        Epic,
        Legend,
    }

    public enum HeroType
    {
        Close,
        Medium,
        Long,
    }
    public enum UIEvent
    {
        Click,
        Press,
    }
    public enum SceneType
    {
        Unknown,
        MainScene,
        GameScene,
    }
    public enum TileType
    {
        Wall,
        Path,
        Install,
        Final,
        Start
    }
    public enum Direction
    {
        None, 
        Up, 
        Down, 
        Left, 
        Right
    }
    public enum State
    {
        Idle,
        Attack,
        Move,
    }
    public enum AtkArange
    {
        Aoe,
        Single,
        CoreCross,
        Rectangle,
        Sector,
        Circle,
    }
    public enum HeroAbility
    {
        Money,
        Atkker,
    }
    public enum ProductType
    {
        Card,
        Goods,
    }
    public enum PayType
    {
        Gold,
        Gem
    }
    public const int HERO_COUNT = 11;
    public const int GAME_LIST_COUNT = 8;
}

public class MeshData
{
    public float angle;
    public float aRange;
    public float Size;
}
public class SkillData
{
    public float Angle;
    public float Attack;
    public float Damage;
    public float Distance;

    public Vector3 TargetPos;
    public Transform TargetTransform;
    
}