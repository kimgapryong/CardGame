using UnityEngine;
using static Define;

public class GameCanvas : UI_Scene
{
    enum Objects
    {
        HeroListContent,
    }
    enum Texts
    {
        Money_Txt,
    }
    enum Images
    {
        Hp_Slider,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

    
        return true;
    }
    public void SetInfo(ClickCotroller click)
    {
        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        for (int i = 0; i < GAME_LIST_COUNT; i++)
        {
            int index = i;
            Manager.UI.MakeSubItem<SpwanFragment>(GetObject((int)Objects.HeroListContent).transform, callback: (spwan) =>
            {
                if (Manager.Game.Heros.Count < index + 1)
                    return;

                HeroData data = Manager.Data.HeroDatas[Manager.Game.Heros[index]];
                spwan.SetInfo(data, click);
                
            });
        }
        
        Manager.Time.moneyAction += (money) =>
        {
            GetText((int)Texts.Money_Txt).text = money.ToString();
        };
        Manager.Time.hpAction = ChangeHealth;

        Manager.Time.SetHp(1000);
        Manager.Time.Money = 500;
    }
    void ChangeHealth(float cur, float max)
    {
        float hpRatio = Mathf.Max(0, cur / max);
        GetImage((int)Images.Hp_Slider).fillAmount = hpRatio;
        
    }

}
