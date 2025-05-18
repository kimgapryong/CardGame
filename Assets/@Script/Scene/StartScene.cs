using UnityEngine;

public class StartScene : BaseScene
{
    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        SceneType = Define.SceneType.MainScene;
        Manager.UI.ShowSceneUI<AllContentCanvas>();
        return true;
    }
}
