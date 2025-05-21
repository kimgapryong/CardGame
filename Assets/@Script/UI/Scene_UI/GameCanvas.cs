using UnityEngine;
using static Define;

public class GameCanvas : UI_Scene
{
    enum Objects
    {
        HeroListContent,
    }

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindObject(typeof(Objects));

        for (int i = 0; i < GAME_LIST_COUNT; i++)
        {

        }
        return true;
    }
}
