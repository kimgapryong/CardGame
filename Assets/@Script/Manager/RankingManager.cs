using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class RankingEntry
{
    public string playerName;
    public string playTime; // ��: "00:12:34"
    public List<int> gameData; // ĳ���� ID ���
    public int totalSeconds; // �񱳿� ���� �� (���� ���ؿ�)
}

[System.Serializable]
public class RankingData
{
    public List<RankingEntry> rankings = new List<RankingEntry>();
}

public class RankingManager
{
    private const string RankingFileName = "ranking.json";
    private string FilePath => Path.Combine(Application.persistentDataPath, RankingFileName);

    public RankingData LoadRankings()
    {
        if (!File.Exists(FilePath))
            return new RankingData();

        string json = File.ReadAllText(FilePath);
        return JsonUtility.FromJson<RankingData>(json);
    }

    public void SaveRanking(string playerName, TimeSpan playTime)
    {
        RankingData data = LoadRankings();

        // "mm:ss" �������� ����
        string formattedTime = string.Format("{0:D2}:{1:D2}",
            playTime.Minutes, playTime.Seconds);

        // ĳ���� ������ ����
        List<int> heroData = new List<int>(Manager.Game.Heros);

        data.rankings.Add(new RankingEntry
        {
            playerName = playerName,
            playTime = formattedTime,
            gameData = heroData,
            totalSeconds = (int)playTime.TotalSeconds
        });

        // ���� ��ƾ ������� ���� (�ð� ��������)
        data.rankings.Sort((a, b) => b.totalSeconds.CompareTo(a.totalSeconds));

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
    }

    public void ClearRanking()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}

