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
    enum GameObjects
    {
        CardFragment,
    }

    Image heroImage;
    Text heroName;

    HeroData _heroData;
    public HeroData HeroData { get { return _heroData; } }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        heroImage = GetImage((int)Images.HeroImage);
        heroName = GetText((int)Texts.CardName);
        GetObject((int)GameObjects.CardFragment).gameObject.BindEvent(ShowCardPop);


        return true;
    }

    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
        Refresh();
    }
    public void Refresh()
    {
        heroName.text = HeroData.HeroName;
        Manager.Resource.LoadAsync<Sprite>(HeroData.Sprite, (sprite) =>
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
