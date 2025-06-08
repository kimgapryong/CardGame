using UnityEngine;
using UnityEngine.SceneManagement;

public class GachaManager 
{
    private Define.SceneType _returnScene;

    /// <summary>
    /// 현재 씬을 킵해두고 가챠씬을 시작
    /// </summary>
    public void StartGacha()
    {
        _returnScene = GameObject.FindFirstObjectByType<BaseScene>().SceneType;
        SceneManager.LoadScene(Define.SceneType.GachaScene.ToString());
        
    }

    /// <summary>
    /// 가챠씬 시작 전 킵해둔 씬으로 복귀
    /// </summary>
    public void ReturnGacha()
    {
        SceneManager.LoadScene(_returnScene.ToString());
    }
}
