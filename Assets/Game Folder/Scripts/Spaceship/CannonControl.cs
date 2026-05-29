using UnityEngine;
using System.Collections;

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
        _waitReloadDuration = new WaitForSeconds(_reloadDuration);
    }

    [Header("Reload Time")]
    [SerializeField] private float _reloadDuration = 5f;


    [Header("Shoot Distances")]
    [SerializeField] private float _maxShootDistance = 100f;
    [SerializeField] private float _maxAimAssistDistance = 500f;

    [Header("Aim Assist Controls")]
    [SerializeField] private float _aimAssistRadius = 10f;
    [SerializeField] private float _aimAssistDisableDistance = 100f; 
    [SerializeField] private LayerMask _trackableLayer;
    private Transform _trackingObject;
    private bool _aimAssist = false;

    [Header("Cannons")]
    [SerializeField] private Transform[] _cannons;

    private Vector2 _crosshairPosition;
    private bool _canShoot = true;
    private Camera _mainCamera;
    private WaitForSeconds _waitReloadDuration;
    private ObjectPooling _objectPooling;

    [SerializeField] private Transform Tracker;


    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        Ray crosshairRay = _mainCamera.ScreenPointToRay(_crosshairPosition);
        RaycastCannonMovement(crosshairRay);
        SpherecastAimAssist(crosshairRay);
    }
    private void RaycastCannonMovement(Ray inputRay)
    {
        Vector3 targetPosition;
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit,_maxShootDistance))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = inputRay.origin + inputRay.direction * _maxShootDistance;
        }

        //Rotates the cannons to point at the crosshair

        RotateCannons(targetPosition);
    }

    //aim assist
    private void SpherecastAimAssist(Ray inputRay)
    {
        RaycastHit sphereHit;
        if (Physics.SphereCast(inputRay, _aimAssistRadius, out sphereHit, _maxAimAssistDistance, _trackableLayer))
        {
            _trackingObject = sphereHit.collider.transform;
            _aimAssist = true;
        }

        if (_aimAssist)
        {
            if (!_trackingObject) return;

            Vector3 trackingObjectScreenPosition = _mainCamera.WorldToScreenPoint(_trackingObject.position);
            float distanceTrackerCrosshair = Vector3.Distance(trackingObjectScreenPosition, _crosshairPosition);
            Tracker.position = trackingObjectScreenPosition;

            if (distanceTrackerCrosshair > _aimAssistDisableDistance)
            {
                _aimAssist = false;
                _trackingObject = null;
            }
        }
        else
        {
            Tracker.position = _crosshairPosition;
        }

    }

    private void RotateCannons(Vector3 target)
    {
        int count = _cannons.Length;
        for (int i = 0; i < count; i++)
        {
            Vector3 direction = target - _cannons[i].position;

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
        int count = _cannons.Length;
        for(int i = 0; i < count; i++)
        {
            var (obj, projectile) = _objectPooling.CannonballPool.SpawnObject(_cannons[i].position, _cannons[i].rotation);
            projectile.InitialiseDirectionOfProjectile(_trackingObject, _aimAssist);
        }

        _canShoot = false;
        StartCoroutine(ReloadProjectile());

    }

    private IEnumerator ReloadProjectile()
    {
        ShootButton.ReloadUIEffect?.Invoke(_reloadDuration);
        yield return _waitReloadDuration;
        _canShoot = true;
    }
    
    
}
