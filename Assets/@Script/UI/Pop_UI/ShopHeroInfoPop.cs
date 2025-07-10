using System;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ShopHeroInfoPop : UI_Popup
{
    //private Sprite _sprite;
    //private HeroData _heroData;
    //private Define.HeroRating _heroRating;
    //private int _priceGold;
    //private int _rewardCount;

    private ShopHeroData _shopHeroData;

    public Action OnClickBuyButton;

    public enum Objects
    {
        CloseButton,
        ProfileImage,
        BuyButton,
        CurrentCountSlider,
    }

    public enum Texts
    {
        HeroNameText,
        HeroNameText2,
        HeroRatingText,
        BuyCostText,
        CurrentCountText,
        RewardCountText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindEvent(GetObject((int)Objects.CloseButton), () => { Manager.UI.ClosePopupUI(this); });
        BindEvent(GetObject((int)Objects.BuyButton), () => { OnClickBuyButton?.Invoke(); });
        GetText((int)Texts.HeroNameText).text = _shopHeroData.heroData.LevelData[0].HeroName;
        GetText((int)Texts.HeroNameText2).text = _shopHeroData.heroData.LevelData[0].HeroName;
        switch (_shopHeroData.heroData.Hero_Rating)
        {

            case HeroRating.Common:
                GetText((int)Texts.HeroRatingText).text = "일반";
                GetText((int)Texts.HeroRatingText).color = Color.gray;
                break;
            case HeroRating.Normal:
                GetText((int)Texts.HeroRatingText).text = "희귀";
                GetText((int)Texts.HeroRatingText).color = Color.yellow;
                break;
            case HeroRating.Epic:
                GetText((int)Texts.HeroRatingText).text = "영웅";
                GetText((int)Texts.HeroRatingText).color = new Color(160f / 255f, 32f / 255f, 240f / 255f);
                break;
            case HeroRating.Legend:
                GetText((int)Texts.HeroRatingText).text = "전설";
                GetText((int)Texts.HeroRatingText).color = Color.red;
                break;
        }

        if (Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].had == false)
        {
            GetText((int)Texts.CurrentCountText).gameObject.SetActive(false);
            GetObject((int)Objects.CurrentCountSlider).SetActive(false);

            Manager.Resource.LoadAsync<Sprite>(_shopHeroData.heroData.LevelData[0].HeroSprite + "_Chain", sprite =>
            {
                GetObject((int)Objects.ProfileImage).GetComponent<Image>().sprite = sprite;
            });
            GetText((int)Texts.RewardCountText).text = $"NEW!";
            int currentCount = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].qnt;
            int currentLevel = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].level;
            int needCount = Manager.Data.UpgradeDatas[_shopHeroData.heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber;
            GetText((int)Texts.CurrentCountText).text = $"{currentCount}/{needCount}";
            GetObject((int)Objects.CurrentCountSlider).GetComponent<Slider>().value = (float)currentCount / needCount;
            GetText((int)Texts.BuyCostText).text = $"{_shopHeroData.priceGold}";
            GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gold >= _shopHeroData.priceGold ? Color.white : Color.red;
        }
        else
        {
            GetText((int)Texts.CurrentCountText).gameObject.SetActive(true);
            GetObject((int)Objects.CurrentCountSlider).SetActive(true);

            Manager.Resource.LoadAsync<Sprite>(_shopHeroData.heroData.LevelData[0].Sprite, sprite =>
            {
                GetObject((int)Objects.ProfileImage).GetComponent<Image>().sprite = sprite;
            });
            GetText((int)Texts.RewardCountText).text = $"x{_shopHeroData.rewardCount}";
            int currentCount = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].qnt;
            int currentLevel = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].level;
            int needCount = Manager.Data.UpgradeDatas[_shopHeroData.heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber;
            GetText((int)Texts.CurrentCountText).text = $"{currentCount}/{needCount}";
            GetObject((int)Objects.CurrentCountSlider).GetComponent<Slider>().value = (float)currentCount / needCount;
            GetText((int)Texts.BuyCostText).text = $"{_shopHeroData.priceGold}";
            GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gold >= _shopHeroData.priceGold ? Color.white : Color.red;
        }
        return true;
    }

    public void SetInfo(ShopHeroData shopHeroData)
    {
        _shopHeroData = shopHeroData;
    }
}
