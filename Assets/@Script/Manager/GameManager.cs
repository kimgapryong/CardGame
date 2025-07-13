using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


[Serializable]
public class GameData
{
    public List<int> Heros;
    public int Gem;
    public int Gold;
    public List<CardData> GotCard = new List<CardData>();
}
public class GameManager
{
    string _path;
    bool IsLoaded { get; set; } = false;

    private GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

    public List<int> Heros { get { return _gameData.Heros; } set { _gameData.Heros = value; } }

    public Dictionary<int, CardData> CardDataDict = new Dictionary<int, CardData>();

    private List<int> startData = new List<int>() {4, 6};
    public void Init()
    {
        _path = Application.persistentDataPath + "/savefile.json";
        if (LoadGame())
            return;
        
        if (Heros == null)
            Heros = new List<int>();

        IsLoaded = true;

        Debug.Log(Manager.Data.HeroDatas[12].HeroID);
        for (int i = 1; i <= Manager.Data.HeroDatas.Values.Count; i++)
        {
            CardData cData = new CardData();
            cData.cardId = i;
            SaveData.GotCard.Add(cData);
            Debug.Log(i);
        }

        foreach (CardData cardData in SaveData.GotCard)
            if (!CardDataDict.ContainsKey(cardData.cardId))
                CardDataDict.Add(cardData.cardId, cardData);

        //히어로 초기 세팅
        foreach(int i in startData)
        {
            Heros.Add(i);

            CardData myCard = CardDataDict[i];
            myCard.had = true;
            CardDataDict[i] = myCard; 
        }
        
        SaveGame();
    }

    public void SaveGame()
    {
        SaveData.GotCard = CardDataDict.Values.ToList();
        string jsonStr = JsonUtility.ToJson(Manager.Game.SaveData);
        File.WriteAllText(_path, jsonStr);
    }

    public bool LoadGame()
    {
        
        if (File.Exists(_path) == false)
            return false;
        Debug.Log("세이브 파일 존재");
        string fileStr = File.ReadAllText(_path);
        if (fileStr == "" || fileStr == null)
        {
            Debug.Log("세이브 파일 비워짐");
            SaveGame();
            fileStr = File.ReadAllText(_path);
        }
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null)
            Manager.Game.SaveData = data;

        if (SaveData.GotCard.Count == 0)
        {
            for (int i = 1; i <= Manager.Data.HeroDatas.Values.Count; i++)
            {
                CardData cData = new CardData();
                cData.cardId = i;
                SaveData.GotCard.Add(cData);
                Debug.Log(i);
            }
            SaveGame();
        }

        foreach (CardData cardData in SaveData.GotCard)
            if (!CardDataDict.ContainsKey(cardData.cardId))
                CardDataDict.Add(cardData.cardId, cardData);

        IsLoaded = true;
        return true;
    }
}
[Serializable]
public struct CardData
{
    //카드 아이디
    public int cardId;
    //카드 레벨
    public int level;
    //카드 개수
    public int qnt;
    //최대 강화 확인
    public bool full;
    //카드 보유 여부
    public bool had;
}