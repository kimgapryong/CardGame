using UnityEngine;
using UnityEngine.UI;

public class NameInputPopup : UI_Popup
{
    enum Inputs { NameInput }
    enum Buttons { ConfirmBtn }

    public System.Action<string> onConfirm;

    private InputField nameInput;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindInput(typeof(Inputs));
        BindButton(typeof(Buttons));

        nameInput = GetInput((int)Inputs.NameInput);
        nameInput.characterLimit = 6; // 최대 6글자 제한

        // 실시간으로 잘라도 되고 (선택 사항)
        nameInput.onValueChanged.AddListener((text) =>
        {
            if (text.Length > 6)
                nameInput.text = text.Substring(0, 6);
        });

        GetButton((int)Buttons.ConfirmBtn).gameObject.BindEvent(() =>
        {
            Debug.Log("여기가 왜 실행되냐");
            string name = nameInput.text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Debug.Log("이름이 비어있습니다.");
                return;
            }

            onConfirm?.Invoke(name);
            Manager.UI.ClosePopupUI(this);
        });

        return true;
    }
}
