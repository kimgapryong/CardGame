using static Define;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Tile
{
    public int x, y;
    public TileType type;
    public Tile(int x, int y, TileType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}

public class MapManager
{
    public int width = 9;
    public int height = 16;
    public Tile[,] map;
    public Dictionary<Vector2Int, Tile> pathDict = new Dictionary<Vector2Int, Tile>();

    private Vector2Int start;
    private Vector2Int end;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@Tile_Root");
            if (root == null)
                root = new GameObject("@Tile_Root");
            return root;
        }
    }

    public void Init()
    {
        GenerateMap();
        InstantiateTiles();
    }

    public void GenerateMap()
    {
        map = new Tile[width, height];

        // 시작/도착 설정
        bool startLeft = UnityEngine.Random.value > 0.5f;
        start = startLeft ? new Vector2Int(1, 1) : new Vector2Int(width - 2, 1);
        end = startLeft ? new Vector2Int(width - 2, height - 2) : new Vector2Int(1, height - 2);

        // 전체 초기화
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = new Tile(x, y, (x == 0 || y == 0 || x == width - 1 || y == height - 1) ? TileType.Wall : TileType.Install);

        // 경로 생성
        List<Vector2Int> path = GeneratePerlinPath(start, end);
        foreach (var pos in path)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= width || pos.y >= height)
                continue;
            if (map[pos.x, pos.y] == null)
                continue;

            map[pos.x, pos.y].type = TileType.Path;
            pathDict[pos] = map[pos.x, pos.y];
        }

        // 시작/끝 타일 설정
        map[start.x, start.y].type = TileType.Start;
        map[end.x, end.y].type = TileType.Final;
    }

    private List<Vector2Int> GeneratePerlinPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Vector2Int current = start;
        path.Add(current);
        visited.Add(current);

        float noiseScale = 0.1f;
        int maxStep = height * 3;  // 유연하게 퍼질 수 있도록

        int step = 0;
        while (current != end && step++ < maxStep)
        {
            List<Vector2Int> candidates = new();

            // 상하좌우 후보
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.left,
                Vector2Int.right
            };

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (next.x <= 0 || next.x >= width - 1 || next.y <= 0 || next.y >= height - 1)
                    continue;
                if (visited.Contains(next))
                    continue;

                float noise = Mathf.PerlinNoise(next.x * noiseScale, next.y * noiseScale);
                float bias = Mathf.Lerp(0f, 1f, (float)(end.y - next.y) / height); // 위로 갈수록 선택 확률 증가
                float score = noise + bias;

                // 확률적으로 후보에 추가
                if (score > 0.3f)
                    candidates.Add(next);
            }

            // 후보가 없다면 강제로 위로
            if (candidates.Count == 0)
                candidates.Add(current + Vector2Int.up);

            Vector2Int chosen = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            path.Add(chosen);
            visited.Add(chosen);
            current = chosen;
        }

        // 끝에 도달 못했다면 강제 연결
        if (current != end)
        {
            while (current != end)
            {
                if (current.x < end.x) current.x++;
                else if (current.x > end.x) current.x--;
                else if (current.y < end.y) current.y++;
                else if (current.y > end.y) current.y--;

                path.Add(current);
            }
        }

        return path;
    }

    public void InstantiateTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = map[x, y];
                int px = x, py = y;
                string spriteKey = "Blue_tile[Blue_TileSet_0]";

                switch (tile.type)
                {
                    case TileType.Wall:
                        spriteKey = "Red_tile[Red_TileSet_0]";
                        break;
                    case TileType.Path:
                        spriteKey = "Green_tile[Green_TileSet_0]";
                        break;
                    case TileType.Start:
                        spriteKey = "Purple_tile[Purple_TileSet_0]";
                        break;
                    case TileType.Final:
                        spriteKey = "Orange_tile[Orange_TileSet_0]";
                        break;
                }

                Manager.Resource.InstantiateSprite("Test_Tile", spriteKey, Root.transform, (obj) =>
                {
                    obj.transform.localPosition = new Vector3Int(px, py);
                });
            }
        }
    }
}
