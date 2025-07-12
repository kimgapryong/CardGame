using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Define;
using static PlacementData;

public class ShopHeroData
{
    public HeroData heroData;
    public int rewardCount;
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

        ShopContent,
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

        Manager.Sound.PlayBGM("Main_Bg");
        
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        RefreshSetCard();
        RefreshCard_Content();

        GetButton((int)Buttons.PartyBtn).gameObject.BindEvent(InfinityAlarm);
        GetButton((int)Buttons.GamePreBtn).gameObject.BindEvent(() => Manager.UI.ShowPopupUI<Pre_Pop>());
        GetButton((int)Buttons.RankingBtn).gameObject.BindEvent(() => Manager.UI.ShowPopupUI<Ranking_Pop>(callback: (pop) =>
        {
            pop.SetInfo(Manager.Rank.LoadRankings());
        }));

        RefreshShopChest();
        RefreshShopHero();
        RefreshShopGold();
        RefreshShopGem();

        return true;
    }

    public void RefreshCard_Content()
    {
        Transform root = GetObject((int)Objects.Card_Content).transform;

        // 기존 카드 제거
        foreach (Transform child in root)
            GameObject.Destroy(child.gameObject);

        // 장착된 카드들
        for (int i = 0; i < HERO_COUNT; i++)
        {
            HeroData _heroData = Manager.Data.HeroDatas[i + 1];
            Manager.UI.MakeSubItem<CardFragment>( 
                root, 
                callback: (card) =>
                {
                    card.SetInfo(_heroData,this);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Card_Content).transform);
                });
        }
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

    public void RefreshShopChest()
    {
        Transform root = GetObject((int)Objects.Chest).transform;
        foreach (Transform child in root)
            Destroy(child.gameObject);

        foreach (ChestData chestData in Manager.Data.ChestDatas.Values)
        {
            Manager.UI.MakeSubItem<ShopChestFragment>( root, callback: (chestFragment) =>
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
                                // 상자깡 팝업 띄우기
                                Manager.UI.ShowPopupUI<ChestPop>(callback: (chestPop) =>
                                {

                                });
                            };
                        });
                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Chest).transform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.ShopContent).transform);
                });
        }
    }
    public void RefreshShopHero()
    {
        Transform root = GetObject((int)Objects.Hero).transform;
        foreach (Transform child in root)
            Destroy(child.gameObject);

        // 미보유 카드들
        for (int i = 1; i <= Manager.Data.HeroDatas.Count; i++)
        {
            if (Manager.Game.CardDataDict[i].had == true)
                continue;

            ShopHeroData shopHeroData = new ShopHeroData()
            {
                heroData = Manager.Data.HeroDatas[i],
                rewardCount = 10,
            };
            Manager.UI.MakeSubItem<ShopHeroFragment>(root, callback: (heroFragment) =>
            {
                heroFragment.SetInfo(shopHeroData);
                BindEvent(heroFragment.gameObject, () =>
                {
                    // 팝업창 띄우기
                    Manager.UI.ShowPopupUI<ShopHeroInfoPop>(callback: (heroPop) =>
                    {
                        heroPop.SetInfo(shopHeroData);
                        heroPop.OnClickBuyButton += () =>
                        {
                            int unitPrice = Manager.Data.PriceDatas[shopHeroData.heroData.Hero_Rating].UnitPrice;
                            int unlockPrice = Manager.Data.PriceDatas[shopHeroData.heroData.Hero_Rating].UnlockPrice;
                            int price = unitPrice * shopHeroData.rewardCount + unlockPrice;
                            if (Manager.Game.SaveData.Gold < price)
                                return;
                            CardData cardData = Manager.Game.CardDataDict[shopHeroData.heroData.HeroID];
                            cardData.had = true;
                            cardData.qnt = shopHeroData.rewardCount;
                            Manager.Game.CardDataDict[shopHeroData.heroData.HeroID] = cardData;
                            Manager.Game.SaveData.Gold -= price;

                            RefreshSetCard();
                            RefreshCard_Content();
                            Manager.UI.ClosePopupUI(heroPop);
                            Manager.Game.SaveGame();

                        };
                    });
                });
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Hero).transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.ShopContent).transform);
            });
        }
        // 보유한 카드들
        for (int i = 1; i <= Manager.Data.HeroDatas.Count; i++)
        {
            if (Manager.Game.CardDataDict[i].had == false)
                continue;

            ShopHeroData shopHeroData = new ShopHeroData()
            {
                heroData = Manager.Data.HeroDatas[i],
                rewardCount = 10,
            };
            Manager.UI.MakeSubItem<ShopHeroFragment>(root, callback: (heroFragment) =>
            {
                heroFragment.SetInfo(shopHeroData);
                BindEvent(heroFragment.gameObject, () =>
                {
                    // 팝업창 띄우기
                    Manager.UI.ShowPopupUI<ShopHeroInfoPop>(callback: (heroPop) =>
                    {
                        heroPop.SetInfo(shopHeroData);
                        heroPop.OnClickBuyButton += () =>
                        {
                            int unitPrice = Manager.Data.PriceDatas[shopHeroData.heroData.Hero_Rating].UnitPrice;
                            int price = unitPrice * shopHeroData.rewardCount;
                            if (Manager.Game.SaveData.Gold < price)
                                return;
                            Manager.Game.SaveData.Gold -= price;
                            CardData cardData = Manager.Game.CardDataDict[shopHeroData.heroData.HeroID];
                            cardData.qnt += shopHeroData.rewardCount;
                            Manager.Game.CardDataDict[shopHeroData.heroData.HeroID] = cardData;
                            Manager.Game.SaveData.Gold -= price;

                            RefreshSetCard();
                            RefreshCard_Content();
                            Manager.UI.ClosePopupUI(heroPop);
                            Manager.Game.SaveGame();

                        };
                    });
                });
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Hero).transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.ShopContent).transform);
            });
        }
    }
    public void RefreshShopGold()
    {
        Transform root = GetObject((int)Objects.Gold).transform;
        foreach (Transform child in root)
            Destroy(child);

        for (int i = 0; i < 3; i++)
        {
            ShopGoldData shopGoldData = new ShopGoldData()
            {
                rewardCount = (i + 1) * 1000,
                priceGem = (i + 1) * 10,
            };
            Manager.UI.MakeSubItem<ShopGoldFragment>( root, callback: (goldFragment) =>
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
                                Manager.Game.SaveGame();
                            };
                        });

                    });
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Gold).transform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.ShopContent).transform);
                });
        }
    }
    public void RefreshShopGem()
    {
        Transform root = GetObject((int)Objects.Gem).transform;
        foreach (Transform child in root)
            Destroy(child);

        for (int i = 0; i < 3; i++)
        {
            ShopGemData shopGemData = new ShopGemData()
            {
                rewardCount = (i + 1) * 10,
                priceGold = (i + 1) * 1000,
            };
            Manager.UI.MakeSubItem<ShopGemFragment>( root, callback: (gemFragment) =>
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
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.Gem).transform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)GetObject((int)Objects.ShopContent).transform);
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

