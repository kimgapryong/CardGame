using UnityEngine;
using static Define;

public class AllContentCanvas : UI_Scene
{
    enum GameObjects
    {
        SetCard,
        CardContent,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        for (int i = 0; i < HERO_COUNT; i++)
        {
            HeroData _heroData = Manager.Data.HeroDatas[i];
        }

        return true;
    }
}
