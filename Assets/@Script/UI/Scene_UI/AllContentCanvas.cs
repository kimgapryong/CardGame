using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;

public class AllContentCanvas : UI_Scene
{
    enum Objects
    {
        SetCard,
        Card_Content,
    }
    enum Buttons
    {
        GameBtn,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        for (int i = 0; i < HERO_COUNT; i++)
        {
            HeroData _heroData = Manager.Data.HeroDatas[i + 1];

            Manager.UI.MakeSubItem<CardFragment>(
                GetObject((int)Objects.Card_Content).transform,
                callback: (card) =>
                {
                    card.SetInfo(_heroData,this);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Card_Content).transform);
                });
        }

        for(int i = 0; i < Manager.Game.Heros.Count; i++)
        {
            HeroData _heroData = Manager.Data.HeroDatas[Manager.Game.Heros[i]];

            Manager.UI.MakeSubItem<CardFragment>(
                GetObject((int)Objects.SetCard).transform,
                callback: (card) =>
                {
                    card.SetInfo(_heroData,this);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.SetCard).transform);
                });
        }
        GetButton((int)Buttons.GameBtn).gameObject.BindEvent(() => SceneManager.LoadScene("GameScene"));

        return true;
    }

    public void RefreshSetCard()
    {
        Transform setCardRoot = GetObject((int)Objects.SetCard).transform;

        // 기존 카드 제거
        foreach (Transform child in setCardRoot)
            GameObject.Destroy(child.gameObject);

        // 다시 추가
        for (int i = 0; i < Manager.Game.Heros.Count; i++)
        {
            HeroData _heroData = Manager.Data.HeroDatas[Manager.Game.Heros[i]];

            Manager.UI.MakeSubItem<CardFragment>(
                setCardRoot,
                callback: (card) =>
                {
                    card.SetInfo(_heroData, this);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)setCardRoot);
                });
        }
    }

}
