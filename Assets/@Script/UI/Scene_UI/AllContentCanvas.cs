using UnityEngine;
using UnityEngine.UI;
using static Define;

public class AllContentCanvas : UI_Scene
{
    enum Objects
    {
        SetCard,
        Card_Content,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));

        for (int i = 0; i < HERO_COUNT; i++)
        {
            int index = i; 
            HeroData _heroData = Manager.Data.HeroDatas[index + 1];

            Manager.UI.MakeSubItem<CardFragment>(
                GetObject((int)Objects.Card_Content).transform,
                callback: (card) =>
                {
                    card.SetInfo(_heroData, index);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Card_Content).transform);
                });
        }


        return true;
    }
}
