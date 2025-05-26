using UnityEngine;

public class Pre_Pop : UI_Popup
{
  enum Buttons
    {
        ExitBtn,
    }
    public override bool Init()
    {
        if(base.Init() == false)    
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => Manager.UI.ClosePopupUI(this));
        return true;
    }

}
