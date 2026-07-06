using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    public static ObjectPooling Instance;

    [Header("Cannon Ball")]
    [SerializeField] private GameObject _cannonballPrefab;
    [SerializeField] private int _cannonballPreloadCount = 10;
    public GenericPoolSystem<ProjectileBase> CannonballPool {  get; private set; }

    [Header("Asteroid Anchor Left")]
    [SerializeField] private GameObject _asteroidAnchorLeftPrefab;
    [SerializeField] private int _asteroidAncorPreloadCount = 10;
    public GenericPoolSystem<ProjectileBase> AsteroidAnchorLeftPool { get; private set; }

    [Header("Asteroid Anchor Right")]
    [SerializeField] private GameObject _asteroidAnchorRightPrefab;
    public GenericPoolSystem<ProjectileBase> AsteroidAnchorRightPool { get; private set; }

    [Header("Explosion")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _explosionPreloadCount = 10;
    public GenericPoolSystem ExplosionPool { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        CannonballPool = new GenericPoolSystem<ProjectileBase>(_cannonballPrefab, _cannonballPreloadCount);
        AsteroidAnchorLeftPool = new GenericPoolSystem<ProjectileBase>(_asteroidAnchorLeftPrefab, _asteroidAncorPreloadCount);
        AsteroidAnchorRightPool = new GenericPoolSystem<ProjectileBase>(_asteroidAnchorRightPrefab, _asteroidAncorPreloadCount);
        ExplosionPool = new GenericPoolSystem(_explosionPrefab, _explosionPreloadCount);
    }
}
