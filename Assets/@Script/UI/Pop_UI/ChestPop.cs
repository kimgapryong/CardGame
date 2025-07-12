using UnityEngine;

public class ChestPop : UI_Popup
{
    public enum Images
    {
        ChestImage,
    }

    ChestData _chestData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        Manager.Resource.LoadSprite(_chestData.OpenSprite, (sprite) =>
        {
            GetImage((int)Images.ChestImage).sprite = sprite;
        });

        return true;
    }
}
