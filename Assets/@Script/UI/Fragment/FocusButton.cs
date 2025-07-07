using UnityEngine;
using UnityEngine.SceneManagement;

public class FocusButton : UI_Base
{
    private enum Objects
    {
        FocusOff,
        FocusOn
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));

        return true;
    }

    public void SetInfo(bool isFocus)
    {
        GetObject((int)Objects.FocusOff).SetActive(isFocus == false);
        GetObject((int)Objects.FocusOff).SetActive(isFocus == true);
    }
}
