using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance { get { Init(); return _instance; } }

    private ResourceManager _resources = new ResourceManager();
    public static ResourceManager Resource { get { return Instance._resources; } }
    private UIManager _ui = new UIManager();
    public static UIManager UI { get { return Instance._ui; } }

    private static void Init()
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
    }
}
