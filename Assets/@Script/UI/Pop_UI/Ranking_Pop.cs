using UnityEngine;

public class Ranking_Pop : UI_Popup
{
    enum Objects { RankContent}
    enum Buttons { ExitBtn}

    private int maxCount = 100;
    RankingData _rankData;

    public override bool Init()
    {
        if(base.Init() == false )
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        int curCount = 0;
        foreach(var data in _rankData.rankings)
        {
            if(curCount >= maxCount)
                break;

            Manager.UI.MakeSubItem<RankFragment>(GetObject((int)Objects.RankContent).transform, callback: (fragment) =>
            {
                fragment.SetInfo(curCount + 1, data);
            });
            curCount++;
        }

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => Manager.UI.ClosePopupUI(this));
        return true;
    }
    public void SetInfo(RankingData rakData)
    {
        _rankData = rakData;
    }
}
