using UnityEngine;

public class Warning_Pop : UI_Popup
{
    string alarm;
    enum Texts
    {
        Alarm_Txt,
    }
   enum Buttons
    {
        Close_Btn,
    }

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetText((int)Texts.Alarm_Txt).text = alarm;
        GetButton((int)Buttons.Close_Btn).gameObject.BindEvent(() => Manager.UI.ClosePopupUI(this));
        return true;
    }

    public void SetAlarm(string alarm)
    {
        this.alarm = alarm;
    }
}
