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
        nameInput.characterLimit = 6; // �ִ� 6���� ����

        // �ǽð����� �߶� �ǰ� (���� ����)
        nameInput.onValueChanged.AddListener((text) =>
        {
            if (text.Length > 6)
                nameInput.text = text.Substring(0, 6);
        });

        GetButton((int)Buttons.ConfirmBtn).gameObject.BindEvent(() =>
        {
            Debug.Log("���Ⱑ �� ����ǳ�");
            string name = nameInput.text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Debug.Log("�̸��� ����ֽ��ϴ�.");
                return;
            }

            onConfirm?.Invoke(name);
            Manager.UI.ClosePopupUI(this);
        });

        return true;
    }
}
