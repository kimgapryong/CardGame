using UnityEngine;

public class SpwanFragment : UI_Base
{
    enum Images
    {
        HeroImage,
        LockImage,
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

        if( _heroData == null)
        {
            GetImage((int)Images.LockImage).gameObject.SetActive(true);
            return true;
        }
        Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[0].Sprite, (sprite) =>
        {
            GetImage((int)Images.HeroImage).sprite = sprite;
        });
        GetImage((int)Images.LockImage).gameObject.SetActive(false);
        GetText((int)Texts.Money_Txt).text = _heroData.LevelData[0].HeroLevelData[0].Upgrade.ToString();
        
       
        return true;
    }
    
    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
    }
}
