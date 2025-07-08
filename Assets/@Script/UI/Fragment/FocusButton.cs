using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FocusButton : UI_Base
{
    public Action OnClickAction;

    private enum Objects
    {
        FocusOff,
        FocusOn
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));

        BindEvent(GetObject((int)Objects.FocusOn), () =>
        {
            OnClickAction?.Invoke();
        });
        BindEvent(GetObject((int)Objects.FocusOff), () =>
        {
            OnClickAction?.Invoke();
        });

        return true;
    }

    public void SetInfo(bool isFocus)
    {
        Init();
        GetObject((int)Objects.FocusOff).SetActive(isFocus == false);
        GetObject((int)Objects.FocusOn).SetActive(isFocus == true);
    }
}
