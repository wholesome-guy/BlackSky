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
    [SerializeField] private float _aimAssistRadius = 10;
    [SerializeField] private float _aimAssistDisableDistance = 100;
    [SerializeField] private Transform[] _cannons;

    [SerializeField] private LayerMask _trackableLayer;
    [SerializeField] private Transform _trackingObject;
    private bool _aimAssist = false;

    private Vector2 _crosshairPosition;
    private bool _canShoot = true;
    private WaitForSeconds _waitReloadduration;
    private ObjectPooling _objectPooling;

    [SerializeField] private Transform Tracker;


    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
    }
    private void Update()
    {
        Ray CrosshairRay = Camera.main.ScreenPointToRay(_crosshairPosition);
        RaycastCannonMovement(CrosshairRay);
        SpherecastAimAssist(CrosshairRay);
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

        
            RotateCannons(targetPosition);
    }
    private void SpherecastAimAssist(Ray inputRay)
    {
        RaycastHit sphereHit;
        if (Physics.SphereCast(inputRay, _aimAssistRadius, out sphereHit, _maxShootDistance, _trackableLayer))
        {
            _trackingObject = sphereHit.collider.transform;
            _aimAssist = true;
        }

        if (_aimAssist)
        {
            Vector3 trackingObjectScreenPosition = Camera.main.WorldToScreenPoint(_trackingObject.position);
            float distanceTrackerCrosshair = Vector3.Distance(trackingObjectScreenPosition, _crosshairPosition);

            Debug.Log(distanceTrackerCrosshair);
            Tracker.position = trackingObjectScreenPosition;

            if (distanceTrackerCrosshair > _aimAssistDisableDistance)
            {
                _aimAssist = false;
                Tracker.position = _crosshairPosition;
                _trackingObject = null;
            }
        }

    }

    private void RotateCannons(Vector3 target)
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
            obj.GetComponent<ProjectileBase>().InitialiseDirectionOfProjectile(_trackingObject, _aimAssist);
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
