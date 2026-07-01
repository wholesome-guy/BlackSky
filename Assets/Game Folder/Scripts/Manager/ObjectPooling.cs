using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    public static ObjectPooling Instance;

    [Header("Cannon Ball")]
    [SerializeField] private GameObject _cannonballPrefab;
    [SerializeField] private int _cannonballPreloadCount = 10;
    public GenericPoolSystem<ProjectileBase> CannonballPool {  get; private set; }

    [Header("Asteroid Anchor")]
    [SerializeField] private GameObject _asteroidAnchorPrefab;
    [SerializeField] private int _asteroidAncorPreloadCount = 10;
    public GenericPoolSystem<ProjectileBase> AsteroidAnchorPool { get; private set; }

    [Header("Asteroid Sticking Anchor")]
    [SerializeField] private GameObject _asteroidStickingAnchorPrefab;
    [SerializeField] private int _asteroidStickingAncorPreloadCount = 10;
    public GenericPoolSystem AsteroidStickingAnchorPool { get; private set; }

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
        AsteroidAnchorPool = new GenericPoolSystem<ProjectileBase>(_asteroidAnchorPrefab,_asteroidAncorPreloadCount);
        AsteroidStickingAnchorPool = new GenericPoolSystem(_asteroidStickingAnchorPrefab,_asteroidStickingAncorPreloadCount);
        ExplosionPool = new GenericPoolSystem(_explosionPrefab, _explosionPreloadCount);
    }
}
