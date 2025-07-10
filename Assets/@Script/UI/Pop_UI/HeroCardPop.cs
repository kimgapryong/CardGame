
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardPop : UI_Popup
{
    bool fullCheck = false;
    bool checkBtn = false;
    SwipeUI swip_UI;
    List<Transform> upgrade_Trans = new List<Transform>();
    UpgradeSystem UpgradeSystem = new UpgradeSystem();
    enum Objects
    {
        Status_Content,
        Upgrade_Count,
    }
    enum Buttons
    {
        Equir_Btn,
        Close_Btn,
        Upgrade_Btn
    }
    enum Texts
    {
        Equir_Txt,
        Upgrade_Txt
    }

    AllContentCanvas _all;
    HeroData _heroData;
    CardFragment _myCard;
    GameObject status_Content;
    GameObject upgrade_Content;

    int requiredCardNumber;
    int currentGainCardNumber;
    List<HeroCardPop_Fragment> heroCardPop_Fragments;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //업그레이드에 필요한 카드 개수를 가져옴
        requiredCardNumber = Manager.Data.UpgradeDatas[_heroData.Hero_Rating].Levels[Manager.Game.CardDataDict[_heroData.HeroID].level].RequiredCardNumber;
        currentGainCardNumber = Manager.Game.CardDataDict[_heroData.HeroID].qnt;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

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
                heroCardPop_Fragments.Add(fragment);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)status_Content.transform);
            });

            Manager.Resource.Instantiate("Upgrade_Fragment", upgrade_Content.transform, (obj) =>
            {
                obj.transform.Find("Num_Txt").GetComponent<Text>().text = (index + 1).ToString();
                upgrade_Trans.Add(obj.transform);
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)upgrade_Content.transform);

                if(upgrade_Trans.Count >= _heroData.LevelData.Count)
                {
                    swip_UI.SetCircelContents(upgrade_Trans, 1);
                    SwipeUI.isWipe = true;
                }
            });
        }

        GetButton((int)Buttons.Close_Btn).gameObject.BindEvent(() =>
        {
            SwipeUI.isWipe = false;
            Manager.UI.ClosePopupUI(this);
        });

        // 버튼 체크
        foreach(var card in Manager.Game.Heros)
        {
            if(card == _heroData.HeroID)
            {
                checkBtn = true;
                break;
            }
        }

        if( checkBtn)
        {
            GetButton((int)Buttons.Equir_Btn).GetComponent<Image>().color = Color.red;
            GetText((int)Texts.Equir_Txt).text = "장착해제";
        }
        else
            GetButton((int)Buttons.Equir_Btn).GetComponent<Image>().color = new Color(72f / 255f, 1f, 0f);

            GetButton((int)Buttons.Equir_Btn).gameObject.BindEvent(() =>
            {
                
                if (checkBtn == false)
                {
                    if (Manager.Game.Heros.Count >= 8)
                        return;

                    SwipeUI.isWipe = false;
                    Manager.UI.ClosePopupUI(this);
                    Manager.Game.Heros.Add(_heroData.HeroID);
                }
                else
                {
                    SwipeUI.isWipe = false;
                    Manager.UI.ClosePopupUI(this);
                    Manager.Game.Heros.Remove(_heroData.HeroID);
                }
                    

                Manager.Game.SaveGame();

                // UI 갱신
                _all?.RefreshSetCard();
            });
        GetText((int)Texts.Upgrade_Txt).text = $"업그레이드({currentGainCardNumber}/{requiredCardNumber})";

        GetButton((int)Buttons.Upgrade_Btn).gameObject.BindEvent(() =>
        {
            if (UpgradeSystem.UpGrade(_heroData.HeroID, this))
            {
                foreach(HeroCardPop_Fragment fragment in heroCardPop_Fragments)
                {
                    fragment.Refresh();
                }
                requiredCardNumber = Manager.Data.UpgradeDatas[_heroData.Hero_Rating].Levels[Manager.Game.CardDataDict[_heroData.HeroID].level].RequiredCardNumber;
                currentGainCardNumber = Manager.Game.CardDataDict[_heroData.HeroID].qnt;
                

                UpgradeSystem.CheckFullAction((cur, full) =>
                {
                    if(cur >= full)
                    {
                        GetText((int)Texts.Upgrade_Txt).text = "Max";
                        fullCheck = true;
                    }
                    else
                        GetText((int)Texts.Upgrade_Txt).text = $"업그레이드({cur}/{requiredCardNumber})";
                });

                _myCard.UpdateSlider(currentGainCardNumber, requiredCardNumber, fullCheck);
            }
        });
       

        return true;
    }
    public void SetInfo(HeroData heroData, AllContentCanvas all, CardFragment myCard)
    {
        _all = all;
        _heroData = heroData;
        _myCard  = myCard;

        heroCardPop_Fragments = new List<HeroCardPop_Fragment>();
    }
}
