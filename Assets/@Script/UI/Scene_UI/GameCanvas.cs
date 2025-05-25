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
            int index = i;  
            Manager.UI.MakeSubItem<SpwanFragment>(GetObject((int)Objects.HeroListContent).transform, callback: (spwan) =>
            {
                HeroData data = Manager.Data.HeroDatas[Manager.Game.Heros[index]];
                if(data == null)
                {
                    Debug.Log("���⿡ ��� ��ư��");
                    return;
                }
                //���⿡ ���ǥ�� ��Ȱ��ȭ
                spwan.SetInfo(data);
            });
        }
        return true;
    }
}
