using System;
using UnityEngine;

public class ShopGemPop : UI_Popup
{
    private ShopGemData _shopGemData;

    public Action OnClickBuyButton;

    public enum Objects
    {
        CloseButton,
        BuyButton,
    }

    public enum Texts
    {
        TitleText,
        RewardCountText,
        BuyCostText,
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

        GetText((int)Texts.TitleText).text = $"{_shopGemData.rewardCount}보석을 구매하시겠습니까?";
        GetText((int)Texts.RewardCountText).text = $"x{_shopGemData.rewardCount}";
        GetText((int)Texts.BuyCostText).text = $"{_shopGemData.priceGold}";
        GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gold >= _shopGemData.priceGold ? Color.white : Color.red;

        BindEvent(GetObject((int)Objects.BuyButton), () => { OnClickBuyButton?.Invoke(); });

        return true;
    }

    public void SetInfo(ShopGemData shopGemData)
    {
        _shopGemData = shopGemData;
    }
}
