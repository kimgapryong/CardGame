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
        Chest, 
        Hero,
        Gold,
        Gem,
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

        #region shop ui

        // 상점에 상자 ui들 생성
        foreach (ChestData chestData in Manager.Data.ChestDatas.Values)
        {
            Manager.UI.MakeSubItem<ShopChestFragment>(
                GetObject((int)Objects.Chest).transform,
                callback: (chestFragment) =>
                {
                    chestFragment.SetInfo(chestData);
                    // UI 눌렀을 때
                    BindEvent(chestFragment.gameObject, () =>
                    {
                        // 팝업창 띄우기
                        Manager.UI.ShowPopupUI<ShopChestInfoPop>(callback: (chestPop) =>
                        {
                            chestPop.SetInfo(chestData);
                            // 팝업 창에서 구매 버튼 눌렀을 때
                            chestPop.OnClickBuyButton += () =>
                            {

                            };
                        });
                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Chest).transform);
                });
        }

        // 상점에 영웅 ui들 생성
        for (int i = 0; i < Manager.Data.HeroDatas.Count; i++)
        {
            HeroData heroData = Manager.Data.HeroDatas[i + 1];

            Manager.UI.MakeSubItem<ShopHeroFragment>(
                GetObject((int)Objects.Hero).transform,
                callback: (heroFragment) =>
                {
                    heroFragment.SetInfo(heroData, 10);
                    // UI 눌렀을 때
                    BindEvent(heroFragment.gameObject, () =>
                    {
                        // 팝업창 띄우기
                        Manager.UI.ShowPopupUI<ShopHeroInfoPop>(callback: (heroPop) =>
                        {
                            heroPop.SetInfo(heroData, 10);
                            // 팝업 창에서 구매 버튼 눌렀을 때
                            heroPop.OnClickBuyButton += () =>
                            {

                            };
                        });

                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Hero).transform);
                });
        }

        // 상점에 골드 ui들 생성
        for (int i = 0; i < 3; i++)
        {
            Manager.UI.MakeSubItem<ShopGoldFragment>(
                GetObject((int)Objects.Gold).transform,
                callback: (goldFragment) =>
                {
                    goldFragment.SetInfo(50, 500);
                    // UI 눌렀을 때
                    BindEvent(goldFragment.gameObject, () =>
                    {
                        //// 팝업창 띄우기
                        //Manager.UI.ShowPopupUI<ShopHeroInfoPop>(callback: (heroPop) =>
                        //{
                        //    heroPop.SetInfo(heroData, 10);
                        //    // 팝업 창에서 구매 버튼 눌렀을 때
                        //    heroPop.OnClickBuyButton += () =>
                        //    {

                        //    };
                        //});

                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Hero).transform);
                });
        }
        #endregion

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
