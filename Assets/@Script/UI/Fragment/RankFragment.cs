using UnityEngine;
using UnityEngine.UI;

public class RankFragment : UI_Base
{
    enum Texts
    {
        Num_Txt,
        Name_Txt,
        Score_Txt
    }
    enum Objects { HeroContent}

    RankingEntry _rankData;
    int mySocre;
    public override bool Init()
    {
        if(base.Init() == false )   
            return false;

        BindText(typeof(Texts));
        BindObject(typeof(Objects));

        GetText((int)Texts.Num_Txt).text = mySocre.ToString();
        GetText((int)Texts.Name_Txt).text = _rankData.playerName;
        GetText((int)Texts.Score_Txt).text = _rankData.playTime;

        foreach(var value in _rankData.gameData)
        {
            Manager.Resource.Instantiate("EmptyImage", GetObject((int)Objects.HeroContent).transform, (obj) =>
            {
                Manager.Resource.LoadAsync<Sprite>(Manager.Data.HeroDatas[value].LevelData[0].Sprite, (sprite) =>
                {
                    obj.GetComponent<Image>().sprite = sprite;
                });
            });
        }
      
        
        return true;
    }
    public void SetInfo(int socre, RankingEntry data)
    {
        mySocre = socre;
        _rankData = data;
    }
}
