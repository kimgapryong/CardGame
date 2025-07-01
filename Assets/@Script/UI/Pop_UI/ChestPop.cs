using UnityEngine;
using UnityEngine.UI;

public class ChestPop : UI_Popup
{
    private ChestData _chestData;

    public enum Objects
    {
        CloseButton,
        ChestImage,
        BuyButton,
    }

    public enum Texts
    {
        ChestNameText,
        CoinCountText,
        CardCountText, 
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

        Manager.Resource.LoadAsync<Sprite>(_chestData.Sprite, sprite =>
        {
            GetObject((int)Objects.ChestImage).GetComponent<Image>().sprite = sprite;
        });

        GetText((int)Texts.ChestNameText).text = _chestData.ChestName;
        GetText((int)Texts.CoinCountText).text = $"x{_chestData.CoinRange[0]}~{_chestData.CoinRange[1]}";
        GetText((int)Texts.CardCountText).text = $"x{_chestData.CardCount}";
        GetText((int)Texts.BuyCostText).text = $"{_chestData.BuyCost}";

        return true;
    }

    public void SetInfo(ChestData chestData)
    {
        _chestData = chestData;
    }
}
