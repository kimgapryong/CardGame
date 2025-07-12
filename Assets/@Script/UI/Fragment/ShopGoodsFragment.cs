using System;
using UnityEngine;
using static Define;


public class ShopGoodsData
{
    public PayType rewardType;
    public int rewardCount;

    public PayType priceType;
    public int priceCount;
}

public class ShopGoodsFragment : UI_Base
{
    public enum Images
    {
        ProfileImage,
        PriceImage,
    }

    public enum Texts
    {
        NameText,
        RewardCountText,
        PriceText,
    }

    ShopGoodsData _shopGoodsData;


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        switch (_shopGoodsData.rewardType)
        {
            case PayType.Gold:
                GetText((int)Texts.NameText).text = $"{_shopGoodsData.rewardCount}골드";
                GetText((int)Texts.RewardCountText).text = $"x{_shopGoodsData.rewardCount}";
                GetText((int)Texts.RewardCountText).color = new Color(1.0f, 1.0f, 0.5f);
                Manager.Resource.LoadSprite("Coin", (sprite) =>
                {
                    GetImage((int)Images.ProfileImage).sprite = sprite;
                });
                break;
            case PayType.Gem:
                GetText((int)Texts.NameText).text = $"{_shopGoodsData.rewardCount}보석";
                GetText((int)Texts.RewardCountText).text = $"x{_shopGoodsData.rewardCount}";
                GetText((int)Texts.RewardCountText).color = new Color(1.0f, 0.5f, 1.0f);
                Manager.Resource.LoadSprite("Gem", (sprite) =>
                {
                    GetImage((int)Images.ProfileImage).sprite = sprite;
                });
                break;
        }

        return true;
    }

    public void SetInfo(ShopGoodsData shopGoodsData)
    {
        _shopGoodsData = shopGoodsData;
    }

    private void RefreshPriceText()
    {
        switch (_shopGoodsData.priceType)
        {
            case PayType.Gold:
                GetText((int)Texts.PriceText).text = $"{_shopGoodsData.priceCount:N0}".Replace(',', ' ');
                GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _shopGoodsData.priceCount ? Color.white : Color.red;
                Manager.Resource.LoadSprite("Coin", (sprite) =>
                {
                    GetImage((int)Images.PriceImage).sprite = sprite;
                });
                break;
            case PayType.Gem:
                GetText((int)Texts.PriceText).text = $"{_shopGoodsData.priceCount:N0}".Replace(',', ' ');
                GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gem >= _shopGoodsData.priceCount ? Color.white : Color.red;
                Manager.Resource.LoadSprite("Gem", (sprite) =>
                {
                    GetImage((int)Images.PriceImage).sprite = sprite;
                });
                break;
        }
    }

    private void LateUpdate()
    {
        RefreshPriceText();
    }
}
