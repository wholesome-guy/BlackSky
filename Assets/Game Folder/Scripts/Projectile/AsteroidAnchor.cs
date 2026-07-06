using System.Collections;
using UnityEngine;

public class AsteroidAnchor : ProjectileBase
{
    [Header("Asteroid Sticking Anchor")]
    [SerializeField] private GameObject _asteroidStickingAnchorPrefab;
    [SerializeField] private float _asteroidStickingAnchorDepth;
    [SerializeField] private string _asteroidsTag;
    [SerializeField] private int _anchorIndex;
    private ObjectPooling _objectPooling;

    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
    }
    protected override void ScheduleDestroyTime(float time)
    {
        _objectPooling.AsteroidAnchorPool.EraseObject(gameObject, time);
    }
    protected override void OnHit(Collision collision)
    {
        if(collision.gameObject.CompareTag(_asteroidsTag))
        {
            _movementEnabled = false;
            ContactPoint interceptPoint = collision.GetContact(0);
            Vector3 invertedNormal = -1 * interceptPoint.normal;
            Quaternion normalVector = Quaternion.LookRotation(invertedNormal);
            Vector3 interceptVector = interceptPoint.point + invertedNormal * _asteroidStickingAnchorDepth;

            GameObject stickingAnchor = _objectPooling.AsteroidStickingAnchorPool.SpawnObject(interceptVector, normalVector);
            stickingAnchor.transform.SetParent(collision.gameObject.transform);
            stickingAnchor.AddComponent<StickingAnchor>().AnchorIndexSetter(_anchorIndex);

            ScheduleDestroyTime(_postHitDestroyTime);

        }
        else
        {
            ScheduleDestroyTime(_postHitDestroyTime);
            StartCoroutine(ExplosionTimer());
        }
    }
    private IEnumerator ExplosionTimer()
    {
        GameObject explosion = _objectPooling.ExplosionPool.SpawnObject(transform.position, Quaternion.identity);

        float timer = 0f;
        while (timer < _explosionDestroyTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _objectPooling.ExplosionPool.EraseObject(explosion, 0.05f);
    }
}
