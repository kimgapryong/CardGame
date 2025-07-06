using System;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ShopGoldPop : UI_Popup
{
    private ShopGoldData _shopGoldData;

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

        GetText((int)Texts.TitleText).text = $"{_shopGoldData.rewardCount}골드를 구매하시겠습니까?";
        GetText((int)Texts.RewardCountText).text = $"x{_shopGoldData.rewardCount}";
        GetText((int)Texts.BuyCostText).text = $"{_shopGoldData.priceGem}";
        GetText((int)Texts.BuyCostText).color = Manager.Game.SaveData.Gem >= _shopGoldData.priceGem ? Color.white : Color.red;

        BindEvent(GetObject((int)Objects.BuyButton), () => { OnClickBuyButton?.Invoke(); });

        return true;
    }

    public void SetInfo(ShopGoldData shopGoldData)
    {
        _shopGoldData = shopGoldData;
    }
}
