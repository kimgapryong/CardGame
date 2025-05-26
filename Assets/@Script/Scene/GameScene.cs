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
        Screen.SetResolution(1920, 1080, false);
        Manager.UI.ShowSceneUI<GameCanvas>(callback: (gameCanvas) =>
        {
            Manager.Resource.Instantiate("ClickController", callback: (obj) =>
            {
                gameCanvas.SetInfo(obj.GetOrAddComponent<ClickCotroller>());
                StartCoroutine(CoWait());
            });
        } );
        
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
        Manager.Time.Start();
        Manager.Obj.Init();
        IsLoad = true;
    }
}
