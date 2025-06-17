using UnityEngine;

public class UpgradeSystem 
{
    public bool UpGrade(int HeroID)
    {
        
        HeroData heroData = Manager.Data.HeroDatas[HeroID];
        int currentLevel = Manager.Game.CardDataDict[HeroID].level;
        int requiredGold = Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].Price;
        if (currentLevel >= Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels.Count - 1)
            return false;
        //현재 가지고 있는 카드의 개수가 업그레이드에 필요한 카드 개수보다 부족할 경우 false return
        if (Manager.Game.CardDataDict[HeroID].qnt < Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber)
        {
            Debug.Log("카드 부족");
            return false;
        }
        //현재 가지고 있는 돈이 업그레이드에 필요한 돈보다 적은 경우 false return
        if (Manager.Game.SaveData.Gold < requiredGold)
        {
            Debug.Log("돈 부족");
            return false;
        }

        
        Manager.Game.SaveData.Gold -= requiredGold;

        Debug.Log($"남은 돈 : {Manager.Game.SaveData.Gold}");
        Debug.Log($"남은 카드 수 : {Manager.Game.CardDataDict[HeroID].qnt - Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber}");
        Manager.Game.CardDataDict[HeroID] = new CardData() { cardId = HeroID, level = currentLevel + 1, qnt = Manager.Game.CardDataDict[HeroID].qnt - Manager.Data.UpgradeDatas[heroData.Hero_Rating].Levels[currentLevel].RequiredCardNumber };

        Manager.Game.SaveGame();
        return true;

    }
}
