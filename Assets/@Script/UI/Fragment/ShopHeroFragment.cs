using Unity.VisualScripting;
using UnityEngine;

public class ShopHeroFragment : UI_Base
{
    public enum Images
    {
        ProfileImage,
    }

    public enum Texts
    {
        NameText,
        RatingText,
        PriceText,
    }

    HeroData _heroData;
    int _priceGold;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
        _priceGold = Manager.Data.HeroRatingPriceDatas[_heroData.Hero_Rating].gold;

        Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[0].Sprite, (sprite) =>
        {
            GetImage((int)Images.ProfileImage).sprite = sprite;
            GetText((int)Texts.NameText).text = _heroData.LevelData[0].HeroName;
            GetText((int)Texts.RatingText).text = _heroData.Hero_Rating.ToString();
            GetText((int)Texts.PriceText).text = _priceGold.ToString();
            GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _priceGold ? Color.white : Color.red;
        });
    }

    private void LateUpdate()
    {
        GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _priceGold ? Color.white : Color.red;
    }
}
