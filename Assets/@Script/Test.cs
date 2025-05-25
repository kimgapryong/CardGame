using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool IsLoad =false;
    void Start()
    {
        StartCoroutine(CoWait());
    }
    private void Update()
    {
        if(!IsLoad)
            return;

        Manager.Obj.Update(Time.deltaTime);
    }
    public IEnumerator CoWait()
    {
        while (!Manager.Data.Loaded())
            yield return null;

        Manager.Map.Init();
        Manager.Time.Start();
        Manager.Obj.Init();
        IsLoad = true;
    }
}
