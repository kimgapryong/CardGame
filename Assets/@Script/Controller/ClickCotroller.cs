using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCotroller : MonoBehaviour
{
    public Grid tilemap;
    public bool heroCur = false;
    public GameObject curHero;

    private Vector3 heroOffset = Vector3.zero;

    private void Start()
    {
        tilemap = GameObject.Find("Grid").GetComponent<Grid>();
    }

    void Update()
    {
        // ����ΰ� ���콺�� ����ٴ�
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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int cellPos = (Vector2Int)tilemap.WorldToCell(mouseWorldPos);

        if (Manager.Map.pathDict.TryGetValue(cellPos, out Tile tile))
        {
            if (tile.type != Define.TileType.Install || tile.hero != null)
            {
                DeleteCurHero();
                return;
            }

            // ���⼭ ���� ��ġ ���� ���� ���� (��: ��ġ Ȯ��)

            heroCur = false;
            curHero = null;
        }
    }

    public void HeroCursor(GameObject hero)
    {
        heroCur = true;
        curHero = hero;

        // ��������Ʈ�� �߽� �� pivot ���� ������ ���
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
    }
}
