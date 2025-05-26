using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFragment : UI_Popup
{
    enum Buttons
    {
        ExitBtn,
        CloseBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() =>
        {
            
            SceneManager.sceneLoaded += OnStartSceneLoaded;
            SceneManager.LoadScene("StartScene");
        });

        
        GetButton((int)Buttons.CloseBtn).gameObject.BindEvent(() =>
        {
            Manager.UI.ClosePopupUI(this);
        });

        return true;
    }

    private void OnStartSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Manager.Time.ResetAll();  // ��� �ʵ�� ��������Ʈ �ʱ�ȭ
        SceneManager.sceneLoaded -= OnStartSceneLoaded;
    }
}
