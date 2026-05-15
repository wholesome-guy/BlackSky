using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class CannonControl : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.OnShoot += ShootProjectile;
        CrosshairMovement.CrosshairPositionAccessor += CrosshairPosition;
    }
    private void OnDisable()
    {
        InputManager.OnShoot -= ShootProjectile;
        CrosshairMovement.CrosshairPositionAccessor -= CrosshairPosition;
    }
    private void Awake()
    {
        _waitReloadduration = new WaitForSeconds(_reloadDuration);
    }

    [SerializeField] private float _reloadDuration = 5f; 
    [SerializeField] private float _maxShootDistance = 100;
    [SerializeField] private Transform[] _cannons;

    [SerializeField] private LayerMask _trackableLayer;
    [SerializeField] private Vector3 _trackingPoint;

    private Vector2 _crosshairPosition;
    private bool _canShoot = true;
    private WaitForSeconds _waitReloadduration;
    private ObjectPooling _objectPooling;


    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
    }
    private void Update()
    {
        RayToCrosshair();
    }
    private void RayToCrosshair()
    {
        Vector3 targetPosition;
        RaycastHit hit;

        Ray inputRay = Camera.main.ScreenPointToRay(_crosshairPosition);

        if (Physics.Raycast(inputRay, out hit,_maxShootDistance,_trackableLayer))
        {
            targetPosition = hit.point;
            _trackingPoint = hit.point;
        }
        else
        {
            targetPosition = inputRay.origin + inputRay.direction * _maxShootDistance;
        }
        RotateToInput(targetPosition);
    }
    private void RotateToInput(Vector3 target)
    {
        for (int i = 0; i < _cannons.Length; i++)
        {
            Vector3 direction = target - _cannons[i].position;

            _cannons[i].forward = direction;

            _cannons[i].rotation = Quaternion.LookRotation(direction);
        }
        
    }

    private void CrosshairPosition(Vector2 position)
    {
        _crosshairPosition = position;
    }
    private void ShootProjectile()
    {
        if (!_canShoot) return;
        for(int i = 0; i< _cannons.Length; i++)
        {
            GameObject obj = _objectPooling.CannonballPool.SpawnObject(_cannons[i].position, _cannons[i].rotation);
            obj.GetComponent<ProjectileBase>().TrackingPoint = _trackingPoint;
        }

        _canShoot = false;
        StartCoroutine(ReloadProjectile());

    }

    private IEnumerator ReloadProjectile()
    {
        ShootButton.ReloadUIEffect.Invoke(_reloadDuration);
        yield return _waitReloadduration;
        _canShoot = true;
    }
    
    
}
