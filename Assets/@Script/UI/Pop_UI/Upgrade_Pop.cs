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

        // ���� �̹��� ����
        Manager.Resource.LoadAsync<Sprite>(curLevel.Sprite, (sprite) =>
        {
            GetImage((int)Images.HeroImage).sprite = sprite;
        });

        // �̸��� �׻� ǥ��
        GetText((int)Texts.Name_Txt).text = curLevel.HeroName;
        GetText((int)Texts.Sell_Txt).text = $"�Ǹ�: {(heroLevel.Upgrade * 0.3f):N0}";

        // �ִ� ������ ���
        if (myHero.curLevel + 1 > _heroData.LevelData.Count - 1)
        {
            GetText((int)Texts.Atk_Txt).text = $"{heroLevel.Attack:N0}";
            GetText((int)Texts.Arg_Txt).text = $"{heroLevel.Arange:N0}";
            GetText((int)Texts.Ats_Txt).text = $"{heroLevel.AtkSpeed:N1}";
            GetText((int)Texts.Upgrade_Txt).text = "�ִ�";
            return;
        }

        LevelData nextLevel = _heroData.LevelData[myHero.curLevel + 1];
        HeroLevelData nextHeroLevel = nextLevel.HeroLevelData;

        GetText((int)Texts.Atk_Txt).text = $"{heroLevel.Attack:N0} �� {nextHeroLevel.Attack:N0}";
        GetText((int)Texts.Arg_Txt).text = $"{heroLevel.Arange:N0} �� {nextHeroLevel.Arange:N0}";
        GetText((int)Texts.Ats_Txt).text = $"{heroLevel.AtkSpeed:N1} �� {nextHeroLevel.AtkSpeed:N1}";
        GetText((int)Texts.Upgrade_Txt).text = $"���׷��̵�: {nextHeroLevel.Upgrade:N0}";
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
            return; //���̾���

        myHero.UpgradeLevel(); // ���׷��̵�
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
