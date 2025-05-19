using UnityEngine;
using UnityEngine.UI;
using static Define;

public class HeroCardPop_Fragment : UI_Base
{
   enum Images
    {
        HeroImage,
        HeroStatusContent

    }
    enum Texts
    {
        HeroName,
        HeroRating_Txt,
        HeroType_Txt,
        HeroIntoduce_Txt,
        Atk_Txt,
        Arg_Txt,
        Aks_Txt,
        Money_Txt
    }

    
    HeroData _heroData;
    LevelData _level;
    HeroLevelData _levelData;

    Text heroT_Txt;
    Text rating_Txt;
    Image heroRating_Image;
    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        rating_Txt = GetText((int)Texts.HeroRating_Txt);
        heroRating_Image = GetImage((int)Images.HeroStatusContent);
        heroT_Txt = GetText((int)Texts.HeroType_Txt);
       
        Refresh();

        return true;
    }

    public void Refresh()
    {
        GetText((int)Texts.HeroName).text = _level.HeroName;
        #region 등급
        switch (_heroData.Hero_Rating)
        {
            case HeroRating.Common:
                heroRating_Image.color = Color.gray;
                rating_Txt.text = $"등급: 일반";
                break;
            case HeroRating.Normal:
                heroRating_Image.color = Color.yellow;
                rating_Txt.text = $"등급: 희귀";
                break;
            case HeroRating.Epic:
                heroRating_Image.color = new Color(160f / 255f, 32f / 255f, 240f / 255f);
                rating_Txt.text = $"등급: 영웅";
                break;
            case HeroRating.Legend:
                heroRating_Image.color = Color.red;
                rating_Txt.text = $"등급: 전설";
                break;
        }
        #endregion

        #region 유형
        switch (_heroData.Arange_Type)
        {
            case HeroType.Close:
                heroT_Txt.text = $"유형: 단거리";
                break;
            case HeroType.Medium:
                heroT_Txt.text = $"유형: 중거리";
                break;
            case HeroType.Long:
                heroT_Txt.text = $"유형: 원거리";
                break;
        }
        #endregion
        Manager.Resource.LoadAsync<Sprite>(_level.Sprite, callback: (sprite) =>
        {
            GetImage((int)Images.HeroImage).sprite = sprite;
        });
        GetText((int)Texts.Atk_Txt).text = _levelData.Attack.ToString();
        GetText((int)Texts.Arg_Txt).text = _levelData.Arange.ToString();
        GetText((int)Texts.Aks_Txt).text = _levelData.AtkSpeed.ToString();  
        GetText((int)Texts.Money_Txt).text = _levelData.Upgrade.ToString();
        GetText((int)Texts.HeroIntoduce_Txt).text = _level.HeroInduce;


    }
    public void SetInfo(HeroData heroData, int myNum)
    {
        _heroData = heroData;
        _level = heroData.LevelData[myNum];
        _levelData = _level.HeroLevelData[0];
    }

}
