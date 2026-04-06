using UnityEngine;
using System.Collections;

public class CannonBall : ProjectileBase
{
    [SerializeField] private float _thrustForce = 1000;
    [SerializeField] private float _torqueForce = 1000;
    [SerializeField] private float _destroyTime = 2f;
    [SerializeField] private float _postHitDestroyTime = 0.05f;
    [SerializeField] private float _explosionDestroyTime = 10f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer _trailRenderer;

    private ObjectPooling _objectPooling;
    private void Awake()
    {
        Thrust = _thrustForce;
        Torque = _torqueForce;
        DestroyTime = _destroyTime;
        PostHitTime = _postHitDestroyTime;
        Rigidbody = _rigidbody;
        TrailRenderer = _trailRenderer;
    }

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

        yield return new WaitForSeconds(_explosionDestroyTime);

        _objectPooling.ExplosionPool.EraseObject(explosion, 0.05f);
    }

}
