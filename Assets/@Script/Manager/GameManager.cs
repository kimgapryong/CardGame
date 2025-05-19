using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class GameData
{
    public List<int> heros;
}
public class GameManager
{
    string _path;
    bool IsLoaded { get; set; } = false;

    private GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

    public List<int> Heros { get { return _gameData.heros; } set { _gameData.heros = value; } }

    public void Init()
    {
        _path = Application.persistentDataPath + "/savefile.json";
        if (LoadGame())
            return;

        if (Heros == null)
            Heros = new List<int>();

        Heros.Add(4);
        Heros.Add(6);


        IsLoaded = true;

        SaveGame();
    }

    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Manager.Game.SaveData);
        File.WriteAllText(_path, jsonStr);
    }

    public bool LoadGame()
    {
        if (File.Exists(_path) == false)
            return false;

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data != null)
            Manager.Game.SaveData = data;

        IsLoaded = true;
        return true;
    }
}
