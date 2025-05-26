using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FindPathEnemy : MonoBehaviour
{
    public TileType[,] map;
    public int mapWidth, mapHeight;
    private float speed;
    public Vector2Int start, end;

    public enum MoveStyle { RightHand, LeftHand, StraightFirst, AStar }
    public MoveStyle style = MoveStyle.RightHand;
    private Vector2Int curDir;
    private Vector2Int curPos;

    private static readonly Vector2Int[] DIRS = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    public void Init(TileType[,] map, int width, int height, Vector2Int start, Vector2Int end, MoveStyle style)
    {
        this.map = map;
        this.mapWidth = width;
        this.mapHeight = height;
        this.start = start;
        this.end = end;
        this.style = style;
        this.curPos = start;

        if (end.x > start.x) curDir = Vector2Int.right;
        else if (end.x < start.x) curDir = Vector2Int.left;
        else if (end.y > start.y) curDir = Vector2Int.up;
        else curDir = Vector2Int.down;
    }

    public void SetInfo(MonsterData data)
    {
        speed = data.Speed;
    }

    Vector2Int[] GetDirectionOrder(Vector2Int dir, MoveStyle style)
    {
        int idx = System.Array.IndexOf(DIRS, dir);

        if (style == MoveStyle.RightHand)
            return new[] { DIRS[(idx + 1) % 4], DIRS[idx], DIRS[(idx + 3) % 4], DIRS[(idx + 2) % 4] };
        if (style == MoveStyle.LeftHand)
            return new[] { DIRS[(idx + 3) % 4], DIRS[idx], DIRS[(idx + 1) % 4], DIRS[(idx + 2) % 4] };
        if (style == MoveStyle.StraightFirst)
            return new[] { DIRS[idx], DIRS[(idx + 1) % 4], DIRS[(idx + 3) % 4], DIRS[(idx + 2) % 4] };
        return DIRS;
    }

    public IEnumerator MoveWithPreferredPath()
    {
        List<Vector2Int> aStarPath = null;
        if (style == MoveStyle.AStar)
            aStarPath = FindAStarPath(curPos, end, map);

        int stuckCount = 0;
        int stuckMax = 8;

        while (true)
        {
            if (curPos == end)
            {
                OnReachDestination(); // µµÂø Ã³¸®
                yield break;
            }

            Vector2Int nextPos = curPos;
            bool found = false;

            if (style == MoveStyle.AStar)
            {
                int idx = aStarPath.IndexOf(curPos);
                if (idx < 0 || idx + 1 >= aStarPath.Count)
                {
                    aStarPath = FindAStarPath(curPos, end, map);
                    if (aStarPath == null || aStarPath.Count < 2)
                    {
                        Destroy(gameObject);
                        yield break;
                    }
                    idx = 0;
                }
                nextPos = aStarPath[idx + 1];
                curDir = nextPos - curPos;
                found = true;
            }
            else
            {
                var dirs = GetDirectionOrder(curDir, style);
                foreach (var d in dirs)
                {
                    Vector2Int check = curPos + d;
                    if (check.x < 0 || check.x >= mapWidth || check.y < 0 || check.y >= mapHeight)
                        continue;

                    var t = map[check.x, check.y];
                    if (t == TileType.Path || t == TileType.Final)
                    {
                        nextPos = check;
                        curDir = d;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var newAStar = FindAStarPath(curPos, end, map);
                    if (newAStar != null && newAStar.Count > 1)
                    {
                        aStarPath = newAStar;
                        style = MoveStyle.AStar;
                        continue;
                    }
                }
            }

            if (!found)
            {
                stuckCount++;
                if (stuckCount > stuckMax)
                {
                    Destroy(gameObject);
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            stuckCount = 0;

            Vector3 target = new Vector3(nextPos.x, nextPos.y, 0);
            yield return MoveToPosition(target, speed);
            curPos = nextPos;
        }
    }

    IEnumerator MoveToPosition(Vector3 target, float moveSpeed)
    {
        Vector3 start = transform.position;
        float t = 0;
        float dist = Vector3.Distance(start, target);

        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, target, t);
            t += Time.deltaTime * moveSpeed / dist;
            yield return null;
        }
        transform.position = target;
    }

    public List<Vector2Int> FindAStarPath(Vector2Int start, Vector2Int end, TileType[,] map)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);
        var openSet = new SimplePriorityQueue<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, int>();
        var fScore = new Dictionary<Vector2Int, int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();
            if (current == end)
            {
                List<Vector2Int> path = new();
                while (cameFrom.ContainsKey(current))
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Add(start);
                path.Reverse();
                return path;
            }

            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;
                if (neighbor.x < 0 || neighbor.x >= w || neighbor.y < 0 || neighbor.y >= h)
                    continue;

                var tile = map[neighbor.x, neighbor.y];
                if (!(tile == TileType.Path || tile == TileType.Start || tile == TileType.Final))
                    continue;

                int tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);

                    if (!openSet.Contains(neighbor))
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }
        return null;
    }

    private int Heuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    private void OnReachDestination()
    {
        MonsterController mon = gameObject.GetComponent<MonsterController>();
        if(mon == null)
            return;
        Manager.Time.CurHp -= mon.CurHp;
        Destroy(gameObject);
    }
}

public class SimplePriorityQueue<T>
{
    private List<(T Item, int Priority)> elements = new();
    public int Count => elements.Count;

    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority));
    }

    public T Dequeue()
    {
        int bestIdx = 0;
        int bestPrio = elements[0].Priority;
        for (int i = 1; i < elements.Count; i++)
        {
            if (elements[i].Priority < bestPrio)
            {
                bestPrio = elements[i].Priority;
                bestIdx = i;
            }
        }
        var best = elements[bestIdx].Item;
        elements.RemoveAt(bestIdx);
        return best;
    }

    public bool Contains(T item)
    {
        return elements.Exists(x => EqualityComparer<T>.Default.Equals(x.Item, item));
    }
}
