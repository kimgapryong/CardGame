using UnityEngine;

public class HeroController : BaseController
{
    HeroData _heroData;
    public void SetInfo(HeroData data)
    {
        _heroData = data;
    }
}
