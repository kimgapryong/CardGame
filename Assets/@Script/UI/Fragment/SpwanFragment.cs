using Unity.VisualScripting;
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
    ClickCotroller _click;
    private bool isLock = false;
    float upgrade;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        FirstRefresh();


        return true;
    }

    public void SetInfo(HeroData heroData, ClickCotroller click)
    {
        _heroData = heroData;
        _click = click;
    }
    void FirstRefresh()
    {
        if (_heroData == null)
        {
            
            GetImage((int)Images.LockImage).gameObject.SetActive(true);
            isLock = true;
            return;
        }

        Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[0].Sprite, (sprite) =>
        {
            GetImage((int)Images.HeroImage).sprite = sprite;
        });
        upgrade = _heroData.LevelData[0].HeroLevelData.Upgrade;
        GetImage((int)Images.LockImage).gameObject.SetActive(false);
        GetText((int)Texts.Money_Txt).text = upgrade.ToString();

        gameObject.BindEvent(SpwanHero);
    }
    void SpwanHero()
    {
        if (isLock)
            return;
        if (_click.heroCur)
            _click.DeleteCurHero();

        Manager.Resource.Instantiate("HeroPrefab", callback: (obj) =>
        {
            Manager.Resource.LoadAsync<Sprite>(_heroData.LevelData[0].HeroSprite, callback: (sprite) =>
            {
                obj.GetOrAddComponent<SpriteRenderer>().sprite = sprite;
                obj.transform.Find("Arange").localScale = Vector3.one * _heroData.LevelData[0].HeroLevelData.Arange;
                obj.transform.Find("AtkArange").localScale = Vector3.one * _heroData.LevelData[0].HeroLevelData.Arange;
                _click.HeroCursor(obj, _heroData);
            });

            obj.transform.Find("AtkArange").GetOrAddComponent<AtkArange>();
            HeroController hero = obj.GetOrAddComponent<HeroController>();
            hero.SetInfo(_heroData);
        });


    }
}
