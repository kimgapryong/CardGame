using static Define;
using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Tile
{
    public int x;
    public int y;
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
    public int width = 10;
    public int height = 15;
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
                root = new GameObject { name = "@Tile_Root" };

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

        // 1. ����/���� ��ġ ����
        bool startLeft = Random.value > 0.5f;
        start = startLeft ? new Vector2Int(0, 0) : new Vector2Int(width - 1, 0);
        end = startLeft ? new Vector2Int(width - 1, height - 1) : new Vector2Int(0, height - 1);

        // 2. ��� Ÿ���� Wall�� �ʱ�ȭ
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = new Tile(x, y, TileType.Wall);
            }
        }

        // 3. �� ���� (���� ����)
        List<Vector2Int> path = GeneratePath(start, end);
        foreach (var pos in path)
        {
            map[pos.x, pos.y].type = TileType.Path;
            pathDict[pos] = map[pos.x, pos.y];
        }

        // 4. ������ ����
        map[end.x, end.y].type = TileType.Final;
    }

    private List<Vector2Int> GeneratePath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Vector2Int current = start;
        path.Add(current);
        visited.Add(current);

        int safety = 1000; // ���ѷ��� ������ ����������

        while (current != end && safety-- > 0)
        {
            List<Vector2Int> nextOptions = new List<Vector2Int>();

            if (current.y < end.y) nextOptions.Add(new Vector2Int(current.x, current.y + 1));
            if (current.x > 0) nextOptions.Add(new Vector2Int(current.x - 1, current.y));
            if (current.x < width - 1) nextOptions.Add(new Vector2Int(current.x + 1, current.y));

            // �湮���� ���� �ĺ��� �߸�
            nextOptions.RemoveAll(pos => visited.Contains(pos));

            if (nextOptions.Count == 0)
            {
                Debug.LogWarning("��� ���� �� �������ϴ�. ���� Ż��");
                break; // �� �� �ִ� ���� ����
            }

            Vector2Int next = nextOptions[Random.Range(0, nextOptions.Count)];
            path.Add(next);
            visited.Add(next);
            current = next;
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

                // ĸó ������ ���� ����
                int px = x;
                int py = y;

                string spriteKey = "Blue_tile[Blue_TileSet_0]"; // ���߿� Ÿ�Ժ��� �ٸ��� �� �� ����

                switch (tile.type)
                {
                    case TileType.Wall:
                    case TileType.Install:
                    case TileType.Path:
                    case TileType.Final:
                        Manager.Resource.InstantiateSprite("Blue", spriteKey, Root.transform, (obj) =>
                        {
                            Debug.Log($"����X: {px}, ����Y: {py}, Ÿ��Type: {tile.type}");
                            obj.transform.localPosition = new Vector3Int(px, py);
                        });
                        break;
                }
            }
        }
    }
}
