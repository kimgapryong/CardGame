using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFragment : UI_Base
{
    enum Images
    {
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

    Image heroImage;
    Text heroName;

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
        heroName = GetText((int)Texts.CardName);
        GetObject((int)Objects.CardFragment).gameObject.BindEvent(ShowCardPop);

        Refresh();
        return true;
    }

    public void SetInfo(HeroData heroData, int myNum)
    {
        Debug.Log(heroData.LevelData.Count);
        Debug.Log(myNum);
        _heroData = heroData;
        _level = heroData.LevelData;
        
    }
    public void Refresh()
    {
        
        heroName.text = _level[0].HeroName;
        Manager.Resource.LoadAsync<Sprite>(_level[0].Sprite, (sprite) =>
        {
            heroImage.sprite = sprite;
        });
    }

    void ShowCardPop()
    {
        Manager.UI.ShowPopupUI<HeroCardPop>(callback: (card) =>
        {
            card.SetInfo(HeroData);
        });
    }
}
