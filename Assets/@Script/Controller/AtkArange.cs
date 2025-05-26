using System.Collections.Generic;
using UnityEngine;

public class AtkArange : MonoBehaviour
{

    public List<MonsterController> targets = new List<MonsterController>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController monster = collision.GetComponent<MonsterController>();
        if (monster != null)
        {
            targets.Add(monster);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController monster = collision.GetComponent<MonsterController>();
        if (monster != null)
        {
            targets.Remove(monster);
        }
    }
}
