using UnityEngine;

public class Ranking_Pop : UI_Popup
{
    enum Objects { RankContent }
    enum Buttons { ExitBtn }

    private int maxCount = 100;
    RankingData _rankData;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        int curCount = 0;
        RectTransform contentRect = GetObject((int)Objects.RankContent).GetComponent<RectTransform>();

        foreach (var data in _rankData.rankings)
        {
            if (curCount >= maxCount)
                break;

            int index = curCount; // 클로저 문제 방지용

            Manager.UI.MakeSubItem<RankFragment>(contentRect.transform, callback: (fragment) =>
            {
                fragment.transform.localPosition = Vector2.zero;
                fragment.SetInfo(index + 1, data);

                // 높이 증가 (220씩)
                float newHeight = 240f * (index + 1);
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
            });

            curCount++;
        }

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() =>
        {
            Manager.UI.ClosePopupUI(this);
        });

        return true;
    }

    public void SetInfo(RankingData rakData)
    {
        _rankData = rakData;
    }
}
