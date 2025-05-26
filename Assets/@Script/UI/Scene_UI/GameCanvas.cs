using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class GameCanvas : UI_Scene
{
    enum Objects
    {
        HeroListContent,
    }
    enum Texts
    {
        Money_Txts,
    }
    enum Images
    {
        Hp_Slider,
        DeathImage,
    }
    enum Buttons
    {
        RePlayBtn,
        GameExitBtn,
        SetBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

  
        Manager.Time.moneyAction = ChangeMoney;
        Manager.Time.hpAction = ChangeHealth;
        Manager.Time.dieAction = DieAction;

        Manager.Time.SetHp(1000);
        Manager.Time.Money = 500;

        // ��ư �̺�Ʈ ����
        GetButton((int)Buttons.RePlayBtn)?.gameObject.BindEvent(() =>
        {
            SceneManager.sceneLoaded += OnGameSceneLoaded;
            SceneManager.LoadScene("GameScene");
        });

        GetButton((int)Buttons.GameExitBtn)?.gameObject.BindEvent(() =>
        {
            SceneManager.sceneLoaded += OnStartSceneLoaded;
            SceneManager.LoadScene("StartScene");
        });

        GetButton((int)Buttons.SetBtn)?.gameObject.BindEvent(() =>
        {
            Manager.UI.ShowPopupUI<ExitFragment>();
        });

        // ó���� DeathImage ��Ȱ��ȭ
        GetImage((int)Images.DeathImage).gameObject.SetActive(false);

        return true;
    }

    
    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnGameSceneLoaded;

        Manager.Time.ResetAll();       // ���� ���� �ʱ�ȭ
        Manager.Time.moneyAction = ChangeMoney;
        Manager.Time.hpAction = ChangeHealth;
        Manager.Time.dieAction = DieAction;

        Manager.Time.SetHp(1000);
        Manager.Time.Money = 500;
        Manager.Time.Start();
    }

    
    private void OnStartSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnStartSceneLoaded;
        Manager.Time.ResetAll(); // ���� �ʱ�ȭ
    }

    public void SetInfo(ClickCotroller click)
    {
        BindObject(typeof(Objects));

        for (int i = 0; i < GAME_LIST_COUNT; i++)
        {
            int index = i;
            Manager.UI.MakeSubItem<SpwanFragment>(GetObject((int)Objects.HeroListContent).transform, callback: (spwan) =>
            {
                if (Manager.Game.Heros.Count < index + 1)
                    return;

                HeroData data = Manager.Data.HeroDatas[Manager.Game.Heros[index]];
                spwan.SetInfo(data, click);
            });
        }
    }

    void ChangeHealth(float cur, float max)
    {
        float hpRatio = Mathf.Max(0, cur / max);
        GetImage((int)Images.Hp_Slider).fillAmount = hpRatio;
    }

    void ChangeMoney(float money)
    {
        GetText((int)Texts.Money_Txts).text = money.ToString();
    }

    void DieAction()
    {
        Manager.Time.Stop();
        GetImage((int)Images.DeathImage).gameObject.SetActive(true);
    }
}
