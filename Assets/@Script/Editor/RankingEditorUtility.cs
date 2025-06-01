using UnityEditor;
using UnityEngine;
using System.IO;

public static class RankingEditorUtility
{
    private const string RankingFileName = "ranking.json";

    [MenuItem("Tools/��ŷ ���� ���� %#d")] // Ctrl + Shift + D (Mac�� Cmd + Shift + D)
    public static void DeleteRankingFile()
    {
        string path = Path.Combine(Application.persistentDataPath, RankingFileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"��ŷ ���� ���� �Ϸ�: {path}");
        }
        else
        {
            Debug.Log("��ŷ ������ �����ϴ�.");
        }
    }
}
