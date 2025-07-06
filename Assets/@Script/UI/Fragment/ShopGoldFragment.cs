using System;
using UnityEngine;
using static Define;

public class ShopGoldFragment : UI_Base
{
    public enum Images
    {
        ProfileImage,
    }

    public enum Texts
    {
        NameText,
        RewardCountText,
        PriceText,
    }

    ShopGoldData _shopGoldData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.NameText).text = $"{_shopGoldData.rewardCount}골드";
        GetText((int)Texts.RewardCountText).text = $"x{_shopGoldData.rewardCount}";
        GetText((int)Texts.PriceText).text = $"{_shopGoldData.priceGem}";

        return true;
    }

    public void SetInfo(ShopGoldData shopGoldData)
    {
        _shopGoldData = shopGoldData;

    }

    private void LateUpdate()
    {
        GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gem >= _shopGoldData.priceGem ? Color.white : Color.red;
    }
}
