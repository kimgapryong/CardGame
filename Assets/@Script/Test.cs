using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Manager.Map.Init();
        Manager.Obj.Init();
    }
    private void Update()
    {
        Manager.Obj.Update(Time.deltaTime);
    }
}
