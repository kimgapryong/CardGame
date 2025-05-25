using static Define;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
    public int width = 32;
    public int height = 14;
    public Tile[,] map;
    public Dictionary<Vector2Int, Tile> pathDict = new Dictionary<Vector2Int, Tile>();

    public Vector2Int start;
    public Vector2Int end;
    public List<Vector2Int> aStarPath = new List<Vector2Int>();

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

        bool startLeft = UnityEngine.Random.value > 0.5f;
        start = startLeft ? new Vector2Int(1, 1) : new Vector2Int(width - 2, 1);
        end = startLeft ? new Vector2Int(width - 2, height - 2) : new Vector2Int(1, height - 2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool isBorder = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                map[x, y] = new Tile(x, y, isBorder ? TileType.Wall : TileType.Install);
            }
        }

        // ---- [수정] 여기서 경로 저장 ----
        aStarPath = GenerateMazePath(start, end);

        foreach (var pos in aStarPath)
        {
            if (pos.x <= 0 || pos.y <= 0 || pos.x >= width || pos.y >= height)
                continue;
            if (map[pos.x, pos.y] == null)
                continue;

            if (map[pos.x, pos.y].type == TileType.Wall)
            {
                if (pos.y - 1 <= 0)
                    continue;

                int x = start.x - pos.x > 0 ? pos.x + 1 : pos.x - 1;
                map[x, pos.y - 1].type = TileType.Path;
                pathDict[pos] = map[x, pos.y - 1];
                continue;
            }

            map[pos.x, pos.y].type = TileType.Path;
            pathDict[pos] = map[pos.x, pos.y];
        }

        map[start.x, start.y].type = TileType.Start;
        map[end.x, end.y].type = TileType.Final;
    }

    private List<Vector2Int> GenerateMazePath(Vector2Int start, Vector2Int end)
    {
        bool[,] visited = new bool[width, height];
        List<Vector2Int> path = new();
        Stack<Vector2Int> stack = new();
        System.Random rng = new();

        stack.Push(start);
        visited[start.x, start.y] = true;
        path.Add(start);

        // 방향 정의 (상하좌우)
        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
        };

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();

            // 방향 섞기 (랜덤)
            directions = directions.OrderBy(d => rng.Next()).ToArray();
            bool moved = false;

            foreach (Vector2Int dir in directions)
            {
                // 이동 조건 설정
                int minStep = (dir.x != 0) ? 4 : 2;
                int maxStep = (dir.x != 0) ? width - 1 : height - 1;
                int stepCount = rng.Next(minStep, Mathf.Min(maxStep, 6)); // 길이는 랜덤, 너무 길면 미로처럼 안됨

                Vector2Int next = current;
                List<Vector2Int> tempPath = new();

                for (int i = 0; i < stepCount; i++)
                {
                    next += dir;
                    if (next.x <= 0 || next.x >= width || next.y <= 0 || next.y >= height)
                        break;

                    if (visited[next.x, next.y])
                        break;

                    tempPath.Add(next);
                }

                if (tempPath.Count == stepCount)
                {
                    foreach (var p in tempPath)
                    {
                        visited[p.x, p.y] = true;
                        path.Add(p);
                    }
                    stack.Push(tempPath.Last());
                    moved = true;
                    break;
                }
            }

            if (!moved)
            {
                stack.Pop(); // 막히면 백트래킹
            }

            // 도착 확인
            if (path.Contains(end))
                break;
        }

        // 도착점으로 이어지는 최종 직선 연결
        Vector2Int last = path.Last();
        while (last != end)
        {
            if (last.x < end.x) last += Vector2Int.right;
            else if (last.x > end.x) last += Vector2Int.left;
            else if (last.y < end.y) last += Vector2Int.up;
            else if (last.y > end.y) last += Vector2Int.down;

            path.Add(last);
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
                string spriteKey = "Install_tile";

                switch (tile.type)
                {
                    case TileType.Wall:
                        spriteKey = "Wall_tile"; break;
                    case TileType.Path:
                        spriteKey = "Path_tile"; break;
                    case TileType.Start:
                        spriteKey = "Start_tile"; break;
                    case TileType.Final:
                        spriteKey = "Final_tile"; break;
                }

                Manager.Resource.LoadAsync<Sprite>(spriteKey, (sp) =>
                {
                    GameObject newObj = new GameObject("Tile");
                    var renderer = newObj.AddComponent<SpriteRenderer>();
                    newObj.transform.SetParent(Root.transform);
                    newObj.transform.localPosition = new Vector3Int(px, py);
                    renderer.sprite = sp;
                });
            }
        }
    }
}
