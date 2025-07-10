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

        return true;
    }

    public void SetInfo(ShopHeroData shopHeroData)
    {
        _shopHeroData = shopHeroData;
    }

    public void LateUpdate()
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




        string spritePath = _shopHeroData.heroData.LevelData[0].HeroSprite; 
        if (Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].had == false)
        {
            GetText((int)Texts.CurrentCountText).gameObject.SetActive(false);
            GetSlider((int)Sliders.CurrentCountSlider).gameObject.SetActive(false);

            GetText((int)Texts.NameText).text = _shopHeroData.heroData.LevelData[0].HeroName;
            Manager.Resource.LoadAsync<Sprite>(_shopHeroData.heroData.LevelData[0].HeroSprite + "_Chain", (sprite) =>
            {
                GetImage((int)Images.ProfileImage).sprite = sprite;
            });
            GetText((int)Texts.RewardCountText).text = $"NEW!";
            GetText((int)Texts.PriceText).text = $"{_shopHeroData.priceGold}";
            GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _shopHeroData.priceGold ? Color.white : Color.red;
        }
        else
        {
            GetText((int)Texts.CurrentCountText).gameObject.SetActive(true);
            GetSlider((int)Sliders.CurrentCountSlider).gameObject.SetActive(true);

            GetText((int)Texts.NameText).text = _shopHeroData.heroData.LevelData[0].HeroName;
            Manager.Resource.LoadAsync<Sprite>(_shopHeroData.heroData.LevelData[0].Sprite, (sprite) =>
            {
                GetImage((int)Images.ProfileImage).sprite = sprite;
            });
            GetText((int)Texts.RewardCountText).text = $"x{_shopHeroData.rewardCount}";
            int currentCount = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].qnt;
            int currentLevel = Manager.Game.CardDataDict[_shopHeroData.heroData.HeroID].level;
            int needCount = Manager.Data.UpgradeDatas[_shopHeroData.heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber;
            GetText((int)Texts.CurrentCountText).text = $"{currentCount}/{needCount}";
            GetSlider((int)Sliders.CurrentCountSlider).value = (float)currentCount / needCount;
            GetText((int)Texts.PriceText).text = $"{_shopHeroData.priceGold}";
            GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _shopHeroData.priceGold ? Color.white : Color.red;
        }

    }
}
