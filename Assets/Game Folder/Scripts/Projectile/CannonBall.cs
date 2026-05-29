using UnityEngine;
using System.Collections;

public class CannonBall : ProjectileBase
{
    private ObjectPooling _objectPooling;

    private void Start()
    {
       _objectPooling = ObjectPooling.Instance;
    }
    protected override void ScheduleDestroyTime(float time)
    {
        _objectPooling.CannonballPool.EraseObject(gameObject, time);
    }
    protected override void OnHit(Collision collision)
    {
        StartCoroutine(ExplosionTimer());
        
        ScheduleDestroyTime(_postHitDestroyTime); 
    }
    private IEnumerator ExplosionTimer()
    {
        GameObject explosion = _objectPooling.ExplosionPool.SpawnObject(transform.position, Quaternion.identity);

        float timer = 0f;
        while (timer < _explosionDestroyTime) 
        { timer += Time.deltaTime;
            yield return null;
        }
        _objectPooling.ExplosionPool.EraseObject(explosion, 0.05f);
    }

}
