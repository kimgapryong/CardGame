using System;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ShopGoodsPop : UI_Popup
{
    private ShopGoodsData _shopGoodsData;

    public Action OnClickBuyButton;

    public enum Texts
    {
        TitleText,
        RewardCountText,
        BuyCostText,
    }

    public enum Images
    {
        ProfileImage,
        PriceImage,
    }

    public enum Objects
    {
        CloseButton,
        BuyButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(Objects));

        BindEvent(GetObject((int)Objects.CloseButton), () => 
        { 
            Manager.UI.ClosePopupUI(this); 
        });

        BindEvent(GetObject((int)Objects.BuyButton), () => 
        { 
            OnClickBuyButton?.Invoke(); 
        });

        switch (_shopGoodsData.rewardType)
        {
            case PayType.Gold:
                GetText((int)Texts.TitleText).text = $"{_shopGoodsData.reward}골드를 구매하시겠습니까?";
                GetText((int)Texts.RewardCountText).text = $"x{_shopGoodsData.reward}";
                Manager.Resource.LoadSprite("Coin", (sprite) =>
                {
                    GetImage((int)Images.ProfileImage).sprite = sprite;
                });
                break;
            case PayType.Gem:
                GetText((int)Texts.TitleText).text = $"{_shopGoodsData.reward}보석을 구매하시겠습니까?";
                GetText((int)Texts.RewardCountText).text = $"x{_shopGoodsData.reward}";
                Manager.Resource.LoadSprite("Gem", (sprite) =>
                {
                    GetImage((int)Images.ProfileImage).sprite = sprite;
                });
                break;
        }
        switch (_shopGoodsData.priceType)
        {
            case PayType.Gold:
                GetText((int)Texts.BuyCostText).text = $"{_shopGoodsData.price}";
                GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gold >= _shopGoodsData.price ? Color.white : Color.red;
                Manager.Resource.LoadSprite("Coin", (sprite) =>
                {
                    GetImage((int)Images.PriceImage).sprite = sprite;
                });
                break;
            case PayType.Gem:
                GetText((int)Texts.BuyCostText).text = $"{_shopGoodsData.price}";
                GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gem >= _shopGoodsData.price ? Color.white : Color.red;
                Manager.Resource.LoadSprite("Gem", (sprite) =>
                {
                    GetImage((int)Images.PriceImage).sprite = sprite;
                });
                break;
        }

        return true;
    }

    public void SetInfo(ShopGoodsData shopGoodsData)
    {
        _shopGoodsData = shopGoodsData;
    }
}
