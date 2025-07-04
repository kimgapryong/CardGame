using System;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ShopHeroInfoPop : UI_Popup
{
    private HeroData _heroData;
    private Define.HeroRating _heroRating;
    private int _priceGold;

    public Action OnClickBuyButton;

    public enum Objects
    {
        CloseButton,
        ProfileImage,
        BuyButton,
    }

    public enum Texts
    {
        HeroNameText,
        HeroNameText2,
        HeroRatingText,
        BuyCostText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindText(typeof(Texts));

        BindEvent(GetObject((int)Objects.CloseButton), () =>
        {
            Manager.UI.ClosePopupUI(this);
        });

        Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[0].Sprite, sprite =>
        {
            GetObject((int)Objects.ProfileImage).GetComponent<Image>().sprite = sprite;
        });

        GetText((int)Texts.HeroNameText).text = _heroData.LevelData[0].HeroName;
        GetText((int)Texts.HeroNameText2).text = _heroData.LevelData[0].HeroName;

        GetText((int)Texts.HeroRatingText).text = _heroRating.ToString();
        switch (_heroRating)
        {

            case HeroRating.Common:
                GetText((int)Texts.HeroRatingText).color = Color.gray;
                break;
            case HeroRating.Normal:
                GetText((int)Texts.HeroRatingText).color = Color.yellow;
                break;
            case HeroRating.Epic:
                GetText((int)Texts.HeroRatingText).color = new Color(160f / 255f, 32f / 255f, 240f / 255f);
                break;
            case HeroRating.Legend:
                GetText((int)Texts.HeroRatingText).color = Color.red;
                break;
        }

        GetText((int)Texts.BuyCostText).text = $"{_priceGold}";
        GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gold >= _priceGold ? Color.white : Color.red;

        BindEvent(GetObject((int)Objects.BuyButton), () =>
        {
            OnClickBuyButton?.Invoke();
        });

        return true;
    }

    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
        _heroRating = _heroData.Hero_Rating;
        _priceGold = Manager.Data.HeroRatingPriceDatas[_heroRating].gold;
    }
}
