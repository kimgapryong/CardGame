using Cysharp.Threading.Tasks;
using UnityEngine;

public class AoeProjectile : MonoBehaviour
{
    public float Speed;

    HeroController owner;
    float aRange;
    Vector2 dir;
    public void SetTarget(HeroController owner, float aRange, Vector2 dir)
    {
        this.owner = owner;
        this.aRange = aRange;
        this.dir = dir;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        MoveProjectile().Forget();
    }

    async UniTaskVoid MoveProjectile()
    {
        while (true)
        {
            float distance = Vector2.Distance(owner.transform.position, transform.position);

            if (distance >= aRange / 2)
                break;

            transform.Translate(dir * Speed * Time.deltaTime);
            await UniTask.Yield();
        }
        Destroy(gameObject);
    }
}
