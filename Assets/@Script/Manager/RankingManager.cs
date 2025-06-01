using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class RankingEntry
{
    public string playerName;
    public string playTime; // 예: "00:12:34"
    public List<int> gameData; // 캐릭터 ID 목록
    public int totalSeconds; // 비교용 내부 값 (정렬 기준용)
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

        // "mm:ss" 형식으로 포맷
        string formattedTime = string.Format("{0:D2}:{1:D2}",
            playTime.Minutes, playTime.Seconds);

        // 캐릭터 데이터 복사
        List<int> heroData = new List<int>(Manager.Game.Heros);

        data.rankings.Add(new RankingEntry
        {
            playerName = playerName,
            playTime = formattedTime,
            gameData = heroData,
            totalSeconds = (int)playTime.TotalSeconds
        });

        // 오래 버틴 순서대로 정렬 (시간 내림차순)
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

