using System.Collections;
using UnityEngine;

public class AsteroidAnchor : ProjectileBase
{
    [Header("Asteroid Sticking Anchor")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private StickingAnchor _stickingAnchor;
    [SerializeField] private Mesh _stickingAnchorMesh;
    [SerializeField] private Material _stickingAnchorMaterial;

    [SerializeField] private float _asteroidStickingAnchorDepth;
    [SerializeField] private string _asteroidsTag;
    private ObjectPooling _objectPooling;

    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
    }
    protected override void ScheduleDestroyTime(float time)
    {

        switch(_stickingAnchor.SpaceshipAnchorIndexGetter())
        {
            case 0:
                _objectPooling.AsteroidAnchorLeftPool.EraseObject(gameObject, time);
                break;
            case 1:
                _objectPooling.AsteroidAnchorRightPool.EraseObject(gameObject, time);
                break;
        }
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

            _meshFilter.mesh = _stickingAnchorMesh;
            _meshRenderer.material = _stickingAnchorMaterial;
            Destroy(_rigidbody);
            Destroy(_collider);
            Destroy(_trailRenderer);
            transform.SetParent(collision.gameObject.transform);
            destroyScheduled = true;

            transform.position = interceptVector;
            transform.rotation = normalVector;

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
