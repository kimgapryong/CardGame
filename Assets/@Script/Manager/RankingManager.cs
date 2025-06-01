using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

[System.Serializable]
public class RankingEntry
{
    public string playerName;
    public string playTime; // 예: "00:12:34"
    public List<int> gameData; // 캐릭터 ID 목록, 최대 8개
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

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)playTime.TotalHours, playTime.Minutes, playTime.Seconds);

        // 캐릭터 데이터 정리 (최대 8개, 부족하면 -1로 채움)
        List<int> heroData = new List<int>(Manager.Game.Heros);
        while (heroData.Count < 8)
            heroData.Add(-1); // 빈 슬롯을 -1로 표시

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
