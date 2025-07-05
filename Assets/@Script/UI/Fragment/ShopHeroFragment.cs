using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ShopHeroFragment : UI_Base
{
    public enum Images
    {
        ProfileImage,
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

    private Sprite _sprite;
    private HeroData _heroData;
    private Define.HeroRating _heroRating;
    private int _priceGold;
    private int _rewardCount;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(HeroData heroData, int rewardCount)
    {
        _heroData = heroData;
        _heroRating = _heroData.Hero_Rating;
        _rewardCount = rewardCount;
        _priceGold = Manager.Data.HeroRatingPriceDatas[_heroRating].gold;

        Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[0].Sprite, (sprite) =>
        {
            _sprite = sprite;
            GetImage((int)Images.ProfileImage).sprite = _sprite;
            GetText((int)Texts.NameText).text = _heroData.LevelData[0].HeroName;
            GetText((int)Texts.RatingText).text = _heroData.Hero_Rating.ToString();
            GetText((int)Texts.RewardCountText).text = $"x{_rewardCount}";
            switch (_heroRating)
            {

                case HeroRating.Common:
                    GetText((int)Texts.RatingText).color = Color.gray;
                    break;
                case HeroRating.Normal:
                    GetText((int)Texts.RatingText).color = Color.yellow;
                    break;
                case HeroRating.Epic:
                    GetText((int)Texts.RatingText).color = new Color(160f / 255f, 32f / 255f, 240f / 255f);
                    break;
                case HeroRating.Legend:
                    GetText((int)Texts.RatingText).color = Color.red;
                    break;
            }
            GetText((int)Texts.PriceText).text = _priceGold.ToString();
        });
    }

    private void LateUpdate()
    {
        GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _priceGold ? Color.white : Color.red;

        //int currentCount = Manager.Game.CardDataDict[_heroData.HeroID].qnt;
        //GetText((int)Texts.CurrentCountText).text = $"{currentCount}";
    }
}
