using UnityEngine;

public class SpwanFragment : UI_Base
{
    enum Images
    {
        HeroImage,
    }
    enum Texts
    {
        Money_Txt,
    }
    HeroData _heroData;
    public override bool Init()
    {
        if(base.Init() == false )
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

       
        return true;
    }
    
    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
    }
}
