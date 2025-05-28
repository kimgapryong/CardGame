using UnityEngine;

public class Upgrade_Pop : UI_Popup
{
    enum Texts
    {
        Name_Txt,
        Atk_Txt,
        Arg_Txt,
        Ats_Txt,
        Sell_Txt,
        Upgrade_Txt,
    }
    enum Images
    {
        HeroImage
    }
    enum Buttons
    {
        SellBtn,
        UpgradeBtn,
        ExitBtn
    }
    HeroData _heroData;
    HeroController myHero;
    public override bool Init()
    {
        if(base.Init() == false)
            return false;
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        return true;
    }

    public void SetInfo(HeroData data, HeroController hero)
    {
        _heroData = data;
        myHero = hero;
    }

}
