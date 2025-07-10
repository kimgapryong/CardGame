using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CardFragment : UI_Base
{
    enum Images
    {
        TextImage,
        HeroImage,
        Upgrade_Slider,
        Chain_Image,
    }
    enum Texts
    {
        CardName,
        Update_Txt,
        Level_Txt
    }
    enum Objects
    {
        CardFragment,
    }
    private bool canCheck;

    Image textImage;
    Image heroImage;
    Text heroName;

    AllContentCanvas _all;
    List<LevelData> _level;
    HeroData _heroData;

    
    public HeroData HeroData { get { return _heroData; } }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(Objects));
        

        heroImage = GetImage((int)Images.HeroImage);
        textImage = GetImage((int)Images.TextImage);
        heroName = GetText((int)Texts.CardName);
        GetObject((int)Objects.CardFragment).gameObject.BindEvent(ShowCardPop);

        Refresh();
        return true;
    }

    public void SetInfo(HeroData heroData, AllContentCanvas all)
    {
        _heroData = heroData;
        _level = heroData.LevelData;
        _all = all;
        
    }
    public void Refresh()
    {
        
        heroName.text = _level[0].HeroName;
        switch (_heroData.Hero_Rating)
        {
            case HeroRating.Common:
                textImage.color = Color.gray;
                break;
            case HeroRating.Normal:
                textImage.color = Color.yellow;
                break;
            case HeroRating.Epic:
                textImage.color = new Color(160f / 255f, 32f / 255f, 240f / 255f);
                break;
            case HeroRating.Legend:
                textImage.color = Color.red;
                break;
        }
        Manager.Resource.LoadAsync<Sprite>(_level[0].Sprite, (sprite) =>
        {
            heroImage.sprite = sprite;
        });

        CardData myCardData = Manager.Game.CardDataDict[_heroData.HeroID];
        UpdateSlider(myCardData.qnt, Manager.Data.UpgradeDatas[_heroData.Hero_Rating].Levels[myCardData.level].RequiredCardNumber, myCardData.full);
        GetImage((int)Images.Chain_Image).gameObject.SetActive(false);
        Manager.Resource.LoadAsync<Sprite>($"{_level[0].HeroSprite}_Chain", (sprite) =>
         {
             GetImage((int)Images.Chain_Image).sprite = sprite;
             if(Manager.Game.CardDataDict.TryGetValue(_heroData.HeroID, out CardData cardData))
             {
                 if(!cardData.had)
                     GetImage((int)Images.Chain_Image).gameObject.SetActive(true);
                 else
                 {
                     GetImage((int)Images.Chain_Image).gameObject.SetActive(false);
                     canCheck = true;
                 }

             }   
         });
    }

    void ShowCardPop()
    {
        if (!canCheck)
        {
            Manager.UI.ShowWarning("아직 획득하지 못한 카드입니다");
            return;
        }
            

        Manager.UI.ShowPopupUI<HeroCardPop>(callback: (card) =>
        {
            card.SetInfo(HeroData, _all, this);
        });
    }

    public void UpdateSlider(int qnt, int upgradeQnt, bool full)
    {
        float slideNormalize = qnt / (float)upgradeQnt;
        GetImage((int)Images.Upgrade_Slider).fillAmount = slideNormalize;

        if (full)
        {
            GetImage((int)Images.Upgrade_Slider).color = new Color(77, 99, 184);
            GetText((int)Texts.Update_Txt).text = "Max";
            return;
        }

        if(slideNormalize >= 1)
            GetImage((int)Images.Upgrade_Slider).color = new Color(117, 230, 76);
        else
            GetImage((int)Images.Upgrade_Slider).color = new Color(224, 215, 215);

        GetText((int)Texts.Level_Txt).text = $"{Manager.Game.CardDataDict[_heroData.HeroID].level}";
        GetText((int)Texts.Update_Txt).text = $"{qnt} / {upgradeQnt}";
    }

}
