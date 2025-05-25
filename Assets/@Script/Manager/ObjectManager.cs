using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectManager
{
    private float spawnInterval = 1f;
    private float monsterSwitchInterval = 10f; // 10초마다 몬스터 종류 변경
    private float timer = 0f;
    private float monsterTypeTimer = 0f;
    private int currentMonsterIndex = 1; // 1~6

    private Vector2Int startPos, endPos;
    private Define.TileType[,] mapArray;
    private int mapWidth, mapHeight;
    private Transform monsterParent;

    public void Init(Transform monsterParent = null)
    {
        this.monsterParent = monsterParent;
        startPos = Manager.Map.start;
        endPos = Manager.Map.end;
        mapArray = ConvertTileArray(Manager.Map.map);
        mapWidth = Manager.Map.width;
        mapHeight = Manager.Map.height;
        timer = spawnInterval;
        monsterTypeTimer = 0f;
        currentMonsterIndex = 1;
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
        monsterTypeTimer += deltaTime;

        // 몬스터 종류 10초마다 바꾸기
        if (monsterTypeTimer >= monsterSwitchInterval)
        {
            monsterTypeTimer = 0f;
            currentMonsterIndex++;
            if (currentMonsterIndex > 6)
                currentMonsterIndex = 1;
        }

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMonster();
        }
    }

    private void SpawnMonster()
    {
        // 공통 프리팹 하나만 사용!
        string prefabKey = $"Monster.Prefab";
        string spriteKey = $"Monster_{currentMonsterIndex}"; // Addressables/Resources에 저장된 스프라이트 이름

        Manager.Resource.Instantiate(prefabKey, monsterParent, (obj) =>
        {
            var find = obj.GetOrAddComponent<FindPathEnemy>();
            var moveStyles = Enum.GetValues(typeof(FindPathEnemy.MoveStyle));
            var style = (FindPathEnemy.MoveStyle)UnityEngine.Random.Range(0, moveStyles.Length);

            float hpMultiplier = Manager.Time.GetHealthMultiplier();

            find.Init(mapArray, mapWidth, mapHeight, startPos, endPos, style);
            find.transform.position = new Vector3(startPos.x, startPos.y, 0);
            find.speed = 3f;
            find.StartCoroutine(find.MoveWithPreferredPath());

            // Sprite만 바꿔준다!
            SpriteRenderer sr = obj.GetOrAddComponent<SpriteRenderer>();

            Manager.Resource.LoadAsync<Sprite>(spriteKey, (sp) =>
            {
                sr.sprite = sp;
            });
        });
    }

    private Define.TileType[,] ConvertTileArray(Tile[,] tileMap)
    {
        int w = tileMap.GetLength(0), h = tileMap.GetLength(1);
        var result = new Define.TileType[w, h];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                result[x, y] = tileMap[x, y].type;
        return result;
    }
}
