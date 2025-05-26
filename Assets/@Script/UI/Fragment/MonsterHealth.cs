using UnityEngine;

public class MonsterHealth : UI_Base
{
   enum Images
    {
        Hp_Slider,
    }
    enum Texts
    {
        Hp_Txt,
    }
    MonsterController _monster;
    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.Hp_Txt).text = $"{_monster.MaxHp}/{_monster.MaxHp}";
        _monster.hpAction = ChangeHealth;

        return true;
    }
    
    public void SetInfo(MonsterController data)
    {
        _monster = data;
    }

    void ChangeHealth(float cur, float max)
    {
        float hpRatio = Mathf.Max(0, cur / max);
        float displayHp = Mathf.Round(cur * 10f) / 10f;

        GetImage((int)Images.Hp_Slider).fillAmount = hpRatio;
        GetText((int)Texts.Hp_Txt).text = $"{displayHp}/{_monster.MaxHp}";
    }
}
