using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCotroller : MonoBehaviour
{
    public Grid tilemap;
    public bool heroCur = false;
    public GameObject curHero;
    public HeroData _heroData;

    private Vector3 heroOffset = Vector3.zero;

    private void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponent<Grid>();
    }

    void Update()
    {
        if (heroCur && curHero != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            curHero.transform.position = mouseWorldPos + heroOffset;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() || !heroCur)
                return;

            TileCheck();
        }
    }

    public void TileCheck()
    {
        Debug.Log("히어로 클릭1222");
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cellPos = (Vector2Int)tilemap.WorldToCell(mouseWorldPos);
       
        if (Manager.Map.pathDict.TryGetValue(cellPos, out Tile tile))
        {
            if(tile.hero != null)
            {
                Debug.Log("히어로 클릭1");
                ShowPlayerStatus(tile.hero, tile);
                return;
            }
            if (Manager.Time.Money < _heroData.LevelData[0].HeroLevelData.Upgrade)
            {
                DeleteCurHero();
                return;
            }
            if (tile.type != Define.TileType.Install || tile.hero != null)
            {

                DeleteCurHero();
                return;
            }


            Vector3 worldOrigin = tilemap.CellToWorld((Vector3Int)cellPos);
            curHero.transform.position = new Vector3(worldOrigin.x, worldOrigin.y + 0.5f, 0f);
            curHero.transform.Find("Arange").gameObject.SetActive(false);

            HeroController hero = curHero.GetComponent<HeroController>();
            hero.SetTile = true;
            tile.hero = curHero;

            Manager.Time.Money -= _heroData.LevelData[0].HeroLevelData.Upgrade;

            heroCur = false;
            heroOffset = Vector3.zero;
            curHero = null;
            _heroData = null;
        }
      
    }
    

    public void HeroCursor(GameObject hero, HeroData data)
    {
        heroCur = true;
        curHero = hero;
        _heroData = data;
        
        SpriteRenderer sr = hero.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector3 boundsCenter = sr.bounds.center;
            Vector3 objectCenter = hero.transform.position;
            heroOffset = objectCenter - boundsCenter;
        }
        else
        {
            heroOffset = Vector3.zero;
        }
    }

    public void DeleteCurHero()
    {
        heroCur = false;

        if (curHero != null)
            Destroy(curHero);

        curHero = null;
        _heroData = null;
    }

    //타일 부분
    void ShowPlayerStatus(GameObject player, Tile tile)
    {
        Debug.Log("히어로 클릭1");
        HeroController hero = player.GetComponent<HeroController>();
        HeroData data = hero._heroData;

        Manager.UI.CloseAllPopupUI();
        Manager.UI.ShowPopupUI<Upgrade_Pop>(callback: (pop) =>
        {
            pop.SetInfo(data,hero, tile);
        });

    }
}
