using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    public static ObjectPooling Instance;

    [Header("Cannon Ball")]
    [SerializeField] private GameObject _cannonballPrefab;
    [SerializeField] private int _cannonballPreloadCount = 10;
    public GenericPoolSystem CannonballPool {  get; private set; }
    
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
        CannonballPool = new GenericPoolSystem(_cannonballPrefab, _cannonballPreloadCount);
        ExplosionPool = new GenericPoolSystem(_explosionPrefab, _explosionPreloadCount);
    }
}
