using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameScene : BaseScene
{
    private bool IsLoad = false;
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.SceneType.GameScene;

        // 현재 해상도 정보 가져오기
        Resolution currentRes = Screen.currentResolution;

        // 현재 해상도의 16:9 비율 해상도 계산
        int height = currentRes.height;
        int width = (int)(height * 16f / 9f);

        // 전체 화면 설정
        Screen.SetResolution(width, height, true); // true == fullscreen

        Manager.UI.ShowSceneUI<GameCanvas>(callback: (gameCanvas) =>
        {
            Manager.Resource.Instantiate("ClickController", callback: (obj) =>
            {
                gameCanvas.SetInfo(obj.GetOrAddComponent<ClickCotroller>());
                StartCoroutine(CoWait());
            });
        });

        return true;
    }

    private void Update()
    {
        if (!IsLoad)
            return;

        Manager.Obj.Update(Time.deltaTime);
    }

    public IEnumerator CoWait()
    {
        while (!Manager.Data.Loaded())
            yield return null;

        Manager.Map.Init();
        Debug.Log("GameScene");
        Debug.Log(Manager.Time);
        Manager.Time.Start();
        Manager.Obj.Init();
        IsLoad = true;
    }
}
