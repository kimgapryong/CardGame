using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem 
{
    HeroData heroData;
    int currentLevel;
    int requiredGold;
    int fullLevel;
    public bool UpGrade(int HeroID, HeroCardPop card_Pop)
    {
        
        heroData = Manager.Data.HeroDatas[HeroID];
        currentLevel = Manager.Game.CardDataDict[HeroID].level;
        requiredGold = Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].Price;
        fullLevel = Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels.Count - 1;

        if (currentLevel >= fullLevel)
        {
            Manager.UI.ShowWarning("이미 카드의 레벨이 최대입니다");
            return false;
        }

        //현재 가지고 있는 카드의 개수가 업그레이드에 필요한 카드 개수보다 부족할 경우 false return
        if (Manager.Game.CardDataDict[HeroID].qnt < Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber)
        {
            Manager.UI.ShowWarning("카드의 갯수가 부족합니다 (뽑기을 이용해보세요)");
            return false;
        }
        //현재 가지고 있는 돈이 업그레이드에 필요한 돈보다 적은 경우 false return
        if (Manager.Game.SaveData.Gold < requiredGold)
        {
            Manager.UI.ShowWarning("돈이 부족합니다");
            return false;
        }

        
        Manager.Game.SaveData.Gold -= requiredGold;

        Debug.Log($"남은 돈 : {Manager.Game.SaveData.Gold}");
        Debug.Log($"남은 카드 수 : {Manager.Game.CardDataDict[HeroID].qnt - Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber}");

        CardData newCard = new CardData() { cardId = HeroID, level = currentLevel + 1, qnt = Manager.Game.CardDataDict[HeroID].qnt - Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber, had = true };
        Manager.Game.CardDataDict[HeroID] = newCard;

        if (currentLevel + 1 >= fullLevel)
            newCard.full = true;

        Manager.Game.SaveGame();
        return true;

    }
    public void CheckFullAction(Action<int, int> callback)
    {
        callback.Invoke(currentLevel + 1, fullLevel);
    }
}
