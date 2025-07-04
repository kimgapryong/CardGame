using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ShopChestFragment : UI_Base
{
    public enum Images
    {
        Image,
    }

    public enum Texts
    {
        NameText,
        PriceText,
    }


    ChestData _chestData;

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Manager.Resource.LoadAsync<Sprite>(_chestData.Sprite, (sprite) =>
        {
            GetImage((int)Images.Image).sprite = sprite;
            GetText((int)Texts.NameText).text = _chestData.ChestName;
            GetText((int)Texts.PriceText).text = $"{_chestData.BuyCost}";
        });

        return true;
    }

    private void LateUpdate()
    {
        GetText((int)Texts.PriceText).color = Manager.Game.SaveData.Gem >= _chestData.BuyCost ? Color.black : Color.red;
    }

    public void SetInfo(ChestData chestData)
    {
        _chestData = chestData;
    }

}
