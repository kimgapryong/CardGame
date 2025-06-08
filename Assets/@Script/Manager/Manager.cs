using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance = null;
    public static Manager Instance { get { Init(); return _instance; } }

    private ResourceManager _resources = new ResourceManager();
    public static ResourceManager Resource { get { return Instance._resources; } }
    private UIManager _ui = new UIManager();
    public static UIManager UI { get { return Instance._ui; } }
    private GameManager _game = new GameManager();
    public static GameManager Game { get { return Instance._game; } }
    private DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance._data; } }
    private MapManager _map = new MapManager();
    public static MapManager Map { get { return Instance._map; } }

    private TimeManager _time = new TimeManager(0.007f);
    public static TimeManager Time { get { return Instance._time; } }
    private ObjectManager _obj = new ObjectManager();   
    public static ObjectManager Obj { get { return Instance._obj; } }

    private RankingManager _rank = new RankingManager();
    public static RankingManager Rank { get { return Instance._rank; } }

    private GachaManager _gacha = new GachaManager();
    public static GachaManager Gacha { get { return Instance._gacha; } }

    public static void Init()
    {
        if(_instance != null)
            return;

        GameObject go = GameObject.Find("@Manager");
        if(go == null)
        {
            go = new GameObject("@Manager");
            go.AddComponent<Manager>();
        }
        _instance = go.GetComponent<Manager>();
        DontDestroyOnLoad(go);

        _instance._data.Init();
        _instance._game.Init();
    }
}
