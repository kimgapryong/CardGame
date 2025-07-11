using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameScene : BaseScene
{
    private bool IsLoad = false;

    Dictionary<Define.HeroRating, int> heroCount = new Dictionary<Define.HeroRating, int>();
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.SceneType.GameScene;

        // 현재 해상도 정보 가져오기
        Resolution currentRes = Screen.currentResolution;

        // 현재 해상도의 16:9 비율 해상도 계산
        int height = currentRes.height;
        int width = (int)(height * 16f / 9f);

        heroCount.Add(Define.HeroRating.Normal, 0);
        heroCount.Add(Define.HeroRating.Common, 0);
        heroCount.Add(Define.HeroRating.Epic, 0);
        heroCount.Add(Define.HeroRating.Legend, 0);
        // 전체 화면 설정
        Screen.SetResolution(width, height, true); // true == fullscreen

        Manager.UI.ShowSceneUI<GameCanvas>(callback: (gameCanvas) =>
        {
            Manager.Resource.Instantiate("ClickController", callback: (obj) =>
            {
                gameCanvas.SetInfo(obj.GetOrAddComponent<ClickCotroller>());
                StartCoroutine(CoWait());
            });
        });

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
        Debug.Log("GameScene");
        Debug.Log(Manager.Time);
        Manager.Time.Start();
        Manager.Obj.Init();
        IsLoad = true;
    }
    public bool PlaceHero(HeroController _hero)
    {
        Define.HeroRating rating = _hero._heroData.Hero_Rating;

        if (Manager.Data.PlacementData[rating].PlaceCount <= heroCount[rating])
        {
            Manager.UI.ShowWarning("이 카드를 더 이상 배치 할 수 없습니다.");
            return false;
        }

        heroCount[rating] += 1;
        return true;
    }
}
