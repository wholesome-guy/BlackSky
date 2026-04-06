using UnityEngine;
using System.Collections;
public class CannonShoot : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.OnShoot += ShootProjectile;
    }
    private void OnDisable()
    {
        InputManager.OnShoot -= ShootProjectile;
    }
    private void Awake()
    {
        _waitReloadduration = new WaitForSeconds(_reloadDuration);
    }

    [SerializeField] private float _reloadDuration = 5f;
    private bool _canShoot = true;
    private WaitForSeconds _waitReloadduration;
    private ObjectPooling _objectPooling;


    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
    }
    private void ShootProjectile()
    {
        if (!_canShoot) return;
        GameObject obj = _objectPooling.CannonballPool.SpawnObject(transform.position,transform.rotation );

        _canShoot = false;
        StartCoroutine(ReloadProjectile());

    }

     private IEnumerator ReloadProjectile()
     {
        yield return _waitReloadduration;
        _canShoot = true;
     }
}
