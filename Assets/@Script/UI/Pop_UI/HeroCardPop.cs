
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardPop : UI_Popup
{
    SwipeUI swip_UI;
    List<Transform> upgrade_Trans = new List<Transform>();
    enum Objects
    {
        Status_Content,
        Upgrade_Count,
    }
    enum Buttons
    {
        Equir_Btn,
        Close_Btn,
    }

    HeroData _heroData;
    GameObject status_Content;
    GameObject upgrade_Content;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        status_Content = GetObject((int)Objects.Status_Content);
        upgrade_Content = GetObject((int)Objects.Upgrade_Count);
        swip_UI = status_Content.GetOrAddComponent<SwipeUI>();

        for(int i = 0; i < _heroData.LevelData.Count; i++)
        {
            int index = i;
            var rt = status_Content.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(1000 + (i * 1000), rt.sizeDelta.y);

            Manager.UI.MakeSubItem<HeroCardPop_Fragment>(status_Content.transform, callback: (fragment) =>
            {
                fragment.SetInfo(_heroData, index);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)status_Content.transform);
            });

            Manager.Resource.Instantiate("Upgrade_Fragment", upgrade_Content.transform, (obj) =>
            {
                obj.transform.Find("Num_Txt").GetComponent<Text>().text = (index + 1).ToString();
                upgrade_Trans.Add(obj.transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)upgrade_Content.transform);

                if(upgrade_Trans.Count >= _heroData.LevelData.Count)
                {
                    swip_UI.SetCircelContents(upgrade_Trans);
                }
            });
        }

        GetButton((int)Buttons.Close_Btn).gameObject.BindEvent(() =>
        {
            Manager.UI.ClosePopupUI(this);
        });
        

       

        return true;
    }
    public void SetInfo(HeroData heroData)
    {
        _heroData = heroData;
    }
}
