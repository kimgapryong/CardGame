using UnityEngine;

public class Upgrade_Pop : UI_Popup
{
    enum Texts
    {
        Name_Txt,
        Atk_Txt,
        Arg_Txt,
        Ats_Txt,
        Sell_Txt,
        Upgrade_Txt,
    }
    enum Images
    {
        HeroImage
    }
    enum Buttons
    {
        SellBtn,
        UpgradeBtn,
        ExitBtn
    }
    HeroData _heroData;
    HeroController myHero;
    Tile curTile;
    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.UpgradeBtn).gameObject.BindEvent(Upgrade);
        GetButton((int)Buttons.SellBtn).gameObject.BindEvent(Sell);
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => Manager.UI.CloseAllPopupUI());
        Refresh();
        return true;
    }
    void Refresh()
    {
        LevelData curLevel = _heroData.LevelData[myHero.curLevel];
        HeroLevelData heroLevel = curLevel.HeroLevelData;

        // 현재 이미지 설정
        Manager.Resource.LoadAsync<Sprite>(curLevel.Sprite, (sprite) =>
        {
            GetImage((int)Images.HeroImage).sprite = sprite;
        });

        // 이름은 항상 표시
        GetText((int)Texts.Name_Txt).text = curLevel.HeroName;
        GetText((int)Texts.Sell_Txt).text = $"판매: {(heroLevel.Upgrade * 0.3f):N0}";

        // 최대 레벨일 경우
        if (myHero.curLevel + 1 > _heroData.LevelData.Count - 1)
        {
            GetText((int)Texts.Atk_Txt).text = $"{heroLevel.Attack:N0}";
            GetText((int)Texts.Arg_Txt).text = $"{heroLevel.Arange:N0}";
            GetText((int)Texts.Ats_Txt).text = $"{heroLevel.AtkSpeed:N1}";
            GetText((int)Texts.Upgrade_Txt).text = "최대";
            return;
        }

        LevelData nextLevel = _heroData.LevelData[myHero.curLevel + 1];
        HeroLevelData nextHeroLevel = nextLevel.HeroLevelData;

        GetText((int)Texts.Atk_Txt).text = $"{heroLevel.Attack:N0} → {nextHeroLevel.Attack:N0}";
        GetText((int)Texts.Arg_Txt).text = $"{heroLevel.Arange:N0} → {nextHeroLevel.Arange:N0}";
        GetText((int)Texts.Ats_Txt).text = $"{heroLevel.AtkSpeed:N1} → {nextHeroLevel.AtkSpeed:N1}";
        GetText((int)Texts.Upgrade_Txt).text = $"업그레이드: {nextHeroLevel.Upgrade:N0}";
    }

    void Sell()
    {
        LevelData curLevel = _heroData.LevelData[myHero.curLevel];
        HeroLevelData heroLevel = curLevel.HeroLevelData;
        float sellMoney = heroLevel.Upgrade * 0.3f;

        Manager.Time.Money += sellMoney;
        curTile.hero = null;
        Destroy(myHero.gameObject);
        Manager.UI.CloseAllPopupUI();
    }
    void Upgrade()
    {
        if (myHero.curLevel + 1 > _heroData.LevelData.Count - 1)
            return;

        LevelData nextLevel = _heroData.LevelData[myHero.curLevel + 1];
        HeroLevelData nextHeroLevel = nextLevel.HeroLevelData;

        if (nextHeroLevel.Upgrade > Manager.Time.Money)
            return; //돈이없다

        myHero.UpgradeLevel(); // 업그레이드
        Refresh();
        Manager.Time.Money -= nextHeroLevel.Upgrade;
    }
    public void SetInfo(HeroData data, HeroController hero, Tile curTile)
    {
        _heroData = data;
        myHero = hero;
        this.curTile = curTile;
    }

}
