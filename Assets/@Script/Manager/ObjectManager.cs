using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager
{
    private float phaseDuration = 20f;
    private float restDuration = 8f;
    private float spawnDelay = 2f;

    private float totalTimer = 0f;
    private float spawnTimer = 0f;
    private bool isResting = false;
    private bool isFirstPhase = true;

    private Vector2Int startPos, endPos;
    private Define.TileType[,] mapArray;
    private int mapWidth, mapHeight;
    private Transform monsterParent;

    private int currentMonsterIndex = 1;
    private int spawnedCount = 0;
    private int spawnCountThisPhase = 0;

    public void Init(Transform monsterParent = null)
    {
        this.monsterParent = monsterParent;
        startPos = Manager.Map.start;
        endPos = Manager.Map.end;
        mapArray = ConvertTileArray(Manager.Map.map);
        mapWidth = Manager.Map.width;
        mapHeight = Manager.Map.height;

        totalTimer = 0f;
        spawnTimer = 0f;
        isResting = false;
        isFirstPhase = true;
        currentMonsterIndex = 1;
        spawnedCount = 0;
        spawnCountThisPhase = GetSpawnCount(currentMonsterIndex);
    }

    public void Update(float deltaTime)
    {
        totalTimer += deltaTime;

        if (isResting)
        {
            if (totalTimer >= restDuration)
            {
                // 다음 Phase 시작
                totalTimer = 0f;
                isResting = false;

                if (isFirstPhase)
                {
                    isFirstPhase = false;
                    currentMonsterIndex = UnityEngine.Random.Range(2, 7);
                }
                else
                {
                    isFirstPhase = true;
                    currentMonsterIndex = 1;
                }

                spawnedCount = 0;
                spawnCountThisPhase = GetSpawnCount(currentMonsterIndex);
                spawnTimer = 0f;
            }

            return;
        }

        if (totalTimer >= phaseDuration)
        {
            totalTimer = 0f;
            isResting = true;
            return;
        }

        // 한 마리씩 spawnDelay 간격으로 소환
        spawnTimer += deltaTime;
        if (spawnedCount < spawnCountThisPhase && spawnTimer >= spawnDelay)
        {
            spawnTimer = 0f;
            spawnedCount++;
            SpawnMonster(currentMonsterIndex);
        }
    }

    private void SpawnMonster(int index)
    {
        string prefabKey = $"Monster.Prefab";
        string spriteKey = $"Monster_{index}";

        Manager.Resource.Instantiate(prefabKey, monsterParent, (obj) =>
        {
            MonsterData data = Manager.Data.MonDatas[index];

            MonsterController enemy = obj.GetOrAddComponent<MonsterController>();
            enemy.SetInfo(data, data.Hp * Manager.Time.GetHealthMultiplier());

            Manager.UI.MakeSubItem<MonsterHealth>(obj.transform, callback: (mon) =>
            {
                mon.SetInfo(enemy);
            });
            

            var find = obj.GetOrAddComponent<FindPathEnemy>();
            find.SetInfo(data);

            var moveStyles = Enum.GetValues(typeof(FindPathEnemy.MoveStyle));
            var style = (FindPathEnemy.MoveStyle)UnityEngine.Random.Range(0, moveStyles.Length);

            find.Init(mapArray, mapWidth, mapHeight, startPos, endPos, style);
            find.transform.position = new Vector3(startPos.x, startPos.y, 0);
            find.StartCoroutine(find.MoveWithPreferredPath());

            SpriteRenderer sr = obj.GetOrAddComponent<SpriteRenderer>();
            Manager.Resource.LoadAsync<Sprite>(spriteKey, (sp) =>
            {
                sr.sprite = sp;
            });
        });
    }

    private int GetSpawnCount(int monsterIndex)
    {
        return monsterIndex switch
        {
            1 => 10,
            2 => 3,
            3 => 4,
            4 => 5,
            5 => 3,
            6 => 2,
            _ => 1,
        };
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
