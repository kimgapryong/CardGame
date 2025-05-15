using UnityEngine;

public class HeroCardPop : UI_Popup
{
    HeroData _heroData;
    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
    }
}
