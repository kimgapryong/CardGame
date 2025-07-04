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
        Card,
        Chest 
    }
    enum Buttons
    {
        GameBtn,
        GamePreBtn,
        RankingBtn,
    }
    enum Texts
    {
        GoldCountText,
        GemCountText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

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
        GetButton((int)Buttons.GamePreBtn).gameObject.BindEvent(() => Manager.UI.ShowPopupUI<Pre_Pop>());
        GetButton((int)Buttons.RankingBtn).gameObject.BindEvent(() => Manager.UI.ShowPopupUI<Ranking_Pop>(callback: (pop) =>
        {
            pop.SetInfo(Manager.Rank.LoadRankings());
        }));

        // 상점에 상자 ui들 생성
        for (int i = 0; i < Manager.Data.ChestDatas.Count; i++)
        {
            ChestData chestData = Manager.Data.ChestDatas[i];

            Manager.UI.MakeSubItem<ChestFragment>(
                GetObject((int)Objects.Chest).transform,
                callback: (chestFragment) =>
                {
                    chestFragment.SetInfo(chestData);
                    BindEvent(chestFragment.gameObject, () =>
                    {
                        Manager.UI.ShowPopupUI<ChestInfoPop>(callback: (chestPop) =>
                        {
                            chestPop.SetInfo(chestData);
                        });
                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Chest).transform);
                });
        }

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

    private void LateUpdate()
    {
        // 골드, 보석 텍스트 갱신
        GetText((int)Texts.GoldCountText).text = $"{Manager.Game.SaveData.Gold:N0}";
        GetText((int)Texts.GemCountText).text = $"{Manager.Game.SaveData.Gem:N0}";

        // 치트키
        if (Input.GetKeyDown(KeyCode.F1))
            Manager.Game.SaveData.Gold += 100;
        if (Input.GetKeyDown(KeyCode.F2))
            Manager.Game.SaveData.Gem += 100;
    }
}
