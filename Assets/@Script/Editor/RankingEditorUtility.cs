using UnityEditor;
using UnityEngine;
using System.IO;

public static class RankingEditorUtility
{
    private const string RankingFileName = "ranking.json";

    [MenuItem("Tools/랭킹 파일 삭제 %#d")] // Ctrl + Shift + D (Mac은 Cmd + Shift + D)
    public static void DeleteRankingFile()
    {
        string path = Path.Combine(Application.persistentDataPath, RankingFileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"랭킹 파일 삭제 완료: {path}");
        }
        else
        {
            Debug.Log("랭킹 파일이 없습니다.");
        }
    }
}
