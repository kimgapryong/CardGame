using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ShopHeroFragment : UI_Base
{
    public enum Images
    {
        ProfileImage,
    }

    public enum Sliders
    {
        CurrentCountSlider,
    }

    public enum Texts
    {
        NameText,
        RatingText,
        PriceText,
        RewardCountText,
        CurrentCountText,
        CurrentLevelText,
    }

    public enum Objects
    {
        SliderArea,
    }

    //private Sprite _sprite;
    //private HeroData _heroData;
    //private Define.HeroRating _heroRating;
    //private int _priceGold;
    //private int _rewardCount;

    ShopHeroData _shopHeroData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindSlider(typeof(Sliders));
        BindObject(typeof(Objects));

        Refresh_HeroRating();

        return true;
    }

    public void SetInfo(ShopHeroData shopHeroData)
    {
        _shopHeroData = shopHeroData;
    }

    public void Refresh_HeroRating()
    {
        switch (_shopHeroData.heroData.Hero_Rating)
        {
            case HeroRating.Common:
                GetText((int)Texts.RatingText).text = "일반";
                GetText((int)Texts.RatingText).color = Color.gray;
                break;
            case HeroRating.Normal:
                GetText((int)Texts.RatingText).text = "희귀";
                GetText((int)Texts.RatingText).color = Color.yellow;
                break;
            case HeroRating.Epic:
                GetText((int)Texts.RatingText).text = "영웅";
                GetText((int)Texts.RatingText).color = new Color(160f / 255f, 32f / 255f, 240f / 255f);
                break;
            case HeroRating.Legend:
                GetText((int)Texts.RatingText).text = "전설";
                GetText((int)Texts.RatingText).color = Color.red;
                break;
        }
    }

    public void LateUpdate()
    {
        if (Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].had == false)
        {
            GetText((int)Texts.NameText).text = _shopHeroData.heroData.LevelData[0].HeroName;
            Manager.Resource.LoadAsync<Sprite>(_shopHeroData.heroData.LevelData[0].HeroSprite + "_Chain", (sprite) =>
            {
                GetImage((int)Images.ProfileImage).sprite = sprite;
            });
            GetText((int)Texts.RewardCountText).text = $"잠금 해제";
            if (_shopHeroData.rewardCount != 0)
                GetText((int)Texts.RewardCountText).text += $"\n+{_shopHeroData.rewardCount}";

            GetObject((int)Objects.SliderArea).SetActive(false);

            int unlockPrice = Manager.Data.PriceDatas[_shopHeroData.heroData.Hero_Rating].UnlockPrice;
            int unitPrice = Manager.Data.PriceDatas[_shopHeroData.heroData.Hero_Rating].UnitPrice;
            int price = unitPrice * _shopHeroData.rewardCount + unlockPrice;
            GetText((int)Texts.PriceText).text = $"{price}";
            GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= price ? Color.white : Color.red;
        }
        else
        {
            GetText((int)Texts.NameText).text = _shopHeroData.heroData.LevelData[0].HeroName;
            Manager.Resource.LoadAsync<Sprite>(_shopHeroData.heroData.LevelData[0].Sprite, (sprite) =>
            {
                GetImage((int)Images.ProfileImage).sprite = sprite;
            });
            GetText((int)Texts.RewardCountText).text = $"+{_shopHeroData.rewardCount}";
            
            int currentLevel = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].level;
            int currentCount = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].qnt;
            int needCount = Manager.Data.UpgradeDatas[_shopHeroData.heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber;
            GetText((int)Texts.CurrentLevelText).text = $"{currentLevel}";
            GetText((int)Texts.CurrentCountText).text = $"{currentCount}/{needCount}";
            GetSlider((int)Sliders.CurrentCountSlider).value = (float)currentCount / needCount;
            GetObject((int)Objects.SliderArea).SetActive(true);

            int unitPrice = Manager.Data.PriceDatas[_shopHeroData.heroData.Hero_Rating].UnitPrice;
            int price = unitPrice * _shopHeroData.rewardCount;
            GetText((int)Texts.PriceText).text = $"{price}";
            GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= price ? Color.white : Color.red;
        }

    }
}
