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

    private int _rewardCount;
    private int _priceCount;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(int rewardCount, int priceCount)
    {
        _rewardCount = rewardCount;
        _priceCount = priceCount;

        GetText((int)Texts.NameText).text = $"{_rewardCount}골드";
        GetText((int)Texts.RewardCountText).text = $"{_rewardCount}";
        GetText((int)Texts.PriceText).text = $"{_priceCount}";
    }

    private void LateUpdate()
    {
        GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gem >= _priceCount ? Color.white : Color.red;
    }
}
