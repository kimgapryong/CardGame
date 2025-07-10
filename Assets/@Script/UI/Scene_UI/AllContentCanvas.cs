using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;

public class ShopHeroData
{
    public HeroData heroData;
    public int rewardCount;
    public int priceGold;
}

public class ShopGoldData
{
    public int rewardCount;
    public int priceGem;
}

public class ShopGemData
{
    public int rewardCount;
    public int priceGold;
}

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
        PartyBtn,
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
        GetButton((int)Buttons.PartyBtn).gameObject.BindEvent(InfinityAlarm);
        GetButton((int)Buttons.GamePreBtn).gameObject.BindEvent(() => Manager.UI.ShowPopupUI<Pre_Pop>());
        GetButton((int)Buttons.RankingBtn).gameObject.BindEvent(() => Manager.UI.ShowPopupUI<Ranking_Pop>(callback: (pop) =>
        {
            pop.SetInfo(Manager.Rank.LoadRankings());
        }));

        #region shop ui

        // 상자
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

        // 영웅
        for (int i = 0; i < Manager.Data.HeroDatas.Count; i++)
        {
            HeroData heroData = Manager.Data.HeroDatas[i + 1];


            PriceData priceData = Manager.Data.PriceDatas[heroData.Hero_Rating];


            ShopHeroData shopHeroData = new ShopHeroData()
            {
                heroData = heroData,
                rewardCount = 10,
                priceGold = priceData.Price,
            };

            Manager.UI.MakeSubItem<ShopHeroFragment>(
                GetObject((int)Objects.Hero).transform,
                callback: (heroFragment) =>
                {
                    heroFragment.SetInfo(shopHeroData);
                    // UI 눌렀을 때
                    BindEvent(heroFragment.gameObject, () =>
                    {
                        // 팝업창 띄우기
                        Manager.UI.ShowPopupUI<ShopHeroInfoPop>(callback: (heroPop) =>
                        {
                            heroPop.SetInfo(shopHeroData);
                            // 팝업 창에서 구매 버튼 눌렀을 때
                            heroPop.OnClickBuyButton += () =>
                            {
                                if (Manager.Game.SaveData.Gold < shopHeroData.priceGold)
                                    return;

                                if (Manager.Game.CardDataDict[shopHeroData.heroData.HeroID].had == false)
                                {
                                    CardData cardData = Manager.Game.CardDataDict[shopHeroData.heroData.HeroID];
                                    cardData.had = true;
                                    cardData.qnt = 0;
                                    Manager.Game.CardDataDict[shopHeroData.heroData.HeroID] = cardData;
                                }
                                else
                                {
                                    Manager.Game.SaveData.Gold -= shopHeroData.priceGold;
                                    CardData cardData = Manager.Game.CardDataDict[shopHeroData.heroData.HeroID];
                                    cardData.qnt += shopHeroData.rewardCount;
                                    Manager.Game.CardDataDict[shopHeroData.heroData.HeroID] = cardData;
                                }
                                RefreshSetCard();
                                Manager.UI.ClosePopupUI(heroPop);
                            };
                        });

                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Hero).transform);
                });
        }

        // 골드
        for (int i = 0; i < 3; i++)
        {
            ShopGoldData shopGoldData = new ShopGoldData()
            {
                rewardCount = (i + 1) * 1000,
                priceGem = (i + 1) * 10,
            };

            Manager.UI.MakeSubItem<ShopGoldFragment>(
                GetObject((int)Objects.Gold).transform,
                callback: (goldFragment) =>
                {
                    goldFragment.SetInfo(shopGoldData);
                    // UI 눌렀을 때
                    BindEvent(goldFragment.gameObject, () =>
                    {
                        // 팝업창 띄우기
                        Manager.UI.ShowPopupUI<ShopGoldPop>(callback: (goldPop) =>
                        {
                            goldPop.SetInfo(shopGoldData);
                            // 팝업 창에서 구매 버튼 눌렀을 때
                            goldPop.OnClickBuyButton += () =>
                            {
                                if (Manager.Game.SaveData.Gem < shopGoldData.priceGem)
                                    return;

                                Manager.Game.SaveData.Gem -= shopGoldData.priceGem;
                                Manager.Game.SaveData.Gold += shopGoldData.rewardCount;
                                Manager.UI.ClosePopupUI(goldPop);
                            };
                        });

                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Hero).transform);
                });
        }

        // 보석
        for (int i = 0; i < 3; i++)
        {
            ShopGemData shopGemData = new ShopGemData()
            {
                rewardCount = (i + 1) * 10,
                priceGold = (i + 1) * 1000,
            };

            Manager.UI.MakeSubItem<ShopGemFragment>(
                GetObject((int)Objects.Gem).transform,
                callback: (gemFragment) =>
                {
                    gemFragment.SetInfo(shopGemData);
                    // UI 눌렀을 때
                    BindEvent(gemFragment.gameObject, () =>
                    {
                        // 팝업창 띄우기
                        Manager.UI.ShowPopupUI<ShopGemPop>(callback: (goldPop) =>
                        {
                            goldPop.SetInfo(shopGemData);
                            // 팝업 창에서 구매 버튼 눌렀을 때
                            goldPop.OnClickBuyButton += () =>
                            {
                                if (Manager.Game.SaveData.Gold < shopGemData.priceGold)
                                    return;

                                Manager.Game.SaveData.Gold -= shopGemData.priceGold;
                                Manager.Game.SaveData.Gem += shopGemData.rewardCount;
                                Manager.UI.ClosePopupUI(goldPop);
                            };
                        });

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
    private void InfinityAlarm()
    {
        if (Manager.Game.Heros.Count <= 0)
        {
            Manager.UI.ShowWarning("카드을 1개 이상 장착하신 후 게임을 플레이해주세요");
            return;
        }
        SceneManager.LoadScene("GameScene");
    }
}

