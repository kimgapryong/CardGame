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
    }
    enum Texts
    {
        CardName,
    }
    enum Objects
    {
        CardFragment,
    }

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
        
    }

    void ShowCardPop()
    {
        Manager.UI.ShowPopupUI<HeroCardPop>(callback: (card) =>
        {
            card.SetInfo(HeroData, _all);
        });
    }
}
