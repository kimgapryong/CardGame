using UnityEngine;

public class ShopGemFragment : UI_Base
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

    ShopGemData _shopGemData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.NameText).text = $"{_shopGemData.rewardCount}보석";
        GetText((int)Texts.RewardCountText).text = $"x{_shopGemData.rewardCount}";
        GetText((int)Texts.PriceText).text = $"{_shopGemData.priceGold}";

        return true;
    }

    public void SetInfo(ShopGemData shopGemData)
    {
        _shopGemData = shopGemData;

    }

    private void LateUpdate()
    {
        GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gold >= _shopGemData.priceGold ? Color.white : Color.red;
    }
}
