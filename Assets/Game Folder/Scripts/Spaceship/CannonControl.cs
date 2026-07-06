using UnityEngine;
using System.Collections;
using System;

public class CannonControl : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.OnShoot += ShootProjectile;
        CrosshairMovement.CrosshairPositionAccessor += CrosshairPosition;
        TargetTracker.AccuracyGetter += GetAccuracy;
        InputManager.OnHoming += HomingSwitch;
        ProjectileSelecter += ProjectileIndexSetter;
    }
    private void OnDisable()
    {
        InputManager.OnShoot -= ShootProjectile;
        CrosshairMovement.CrosshairPositionAccessor -= CrosshairPosition;
        TargetTracker.AccuracyGetter -= GetAccuracy;
        InputManager.OnHoming -= HomingSwitch;
        ProjectileSelecter -= ProjectileIndexSetter;

    }
    private void Awake()
    {
        _waitReloadDuration = new WaitForSeconds(_reloadDuration);
    }

    [Header("Reload Time")]
    [SerializeField] private float _reloadDuration = 5f;
    [SerializeField] private float _cannonOffset = 5f;

    [Header("Shoot Distances")]
    [SerializeField] private float _maxShootDistance = 100f;
    [SerializeField] private float _maxAimAssistDistance = 500f;

    [Header("Aim Assist Controls")]
    [SerializeField] private float _aimAssistRadius = 10f;
    [SerializeField] private float _aimAssistDisableDistance = 100f; 
    [SerializeField] private LayerMask _trackableLayer;
    private float _accuracy;
    private Transform _trackingObject;
    public bool _aimAssist = false;
    private bool _masterHoming = false;

    [Header("Cannons")]
    [SerializeField] private Transform[] _cannons;

    private Vector2 _crosshairPosition;
    private bool _canShoot = true;
    private Camera _mainCamera;
    private int _projectileIndex;
    private WaitForSeconds _waitReloadDuration;
    private ObjectPooling _objectPooling;
    private bool _isTrackerActive = false;

    public static Action<int> ProjectileSelecter;

    private void Start()
    {
        _objectPooling = ObjectPooling.Instance;
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        Ray crosshairRay = _mainCamera.ScreenPointToRay(_crosshairPosition);
        RaycastCannonMovement(crosshairRay);
        if (_masterHoming)
        {
            SpherecastAimAssist(crosshairRay);
        }
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

        if (_aimAssist && _trackingObject)
        {
            Vector2 trackingObjectScreenPosition = _mainCamera.WorldToScreenPoint(_trackingObject.position);
            float distanceTrackerCrosshair = Vector2.Distance(trackingObjectScreenPosition, _crosshairPosition);

            if (!_isTrackerActive)
            {
                TargetTracker.OnTrackerActiveSwitch?.Invoke(true);
                _isTrackerActive = true;
            }
            else
            {
                TargetTracker.OnTrackerSetPosition?.Invoke(trackingObjectScreenPosition);
            }


            if (distanceTrackerCrosshair > _aimAssistDisableDistance && _isTrackerActive)
            {
                _aimAssist = false;
                _trackingObject = null;
                TargetTracker.OnTrackerActiveSwitch?.Invoke(false);
                _isTrackerActive = false;
                
            }
        }
        

    }

    private void RotateCannons(Vector3 target)
    {
        int count = _cannons.Length;
        for (int i = 0; i < count; i++)
        {
            Vector3 offsetVector = Vector3.right;

            if(i % 2 == 0)
            {
                offsetVector = offsetVector * _cannonOffset * -1;
            }
            else
            {
                offsetVector = offsetVector * _cannonOffset;
            }
                Vector3 direction = target - _cannons[i].position + offsetVector;

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
            var (obj, projectile) = SelectProjectile(_projectileIndex,i);
            if (!_trackingObject||!_masterHoming)
            {
                _aimAssist = false;
            }
            projectile.InitialiseProjectileTrajectory(_trackingObject, _aimAssist,_accuracy);
        }

        _canShoot = false;
        StartCoroutine(ReloadProjectile());

    }
    private void ProjectileIndexSetter(int i)
    {
        _projectileIndex = i;
    }
    private (GameObject gameObject,ProjectileBase projectile) SelectProjectile(int projectileIndex,int cannonIndex)
    {
        switch(projectileIndex)
        {
            case 0:
                return _objectPooling.CannonballPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
            case 1:
                if(cannonIndex % 2 == 0)
                {
                   return _objectPooling.AsteroidAnchorLeftPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
                }
                else
                {
                    return _objectPooling.AsteroidAnchorRightPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
                }
            case 2:
                return _objectPooling.CannonballPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
            case 3:
                return _objectPooling.CannonballPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
            case 4:
                return _objectPooling.CannonballPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
            default:
                return _objectPooling.CannonballPool.SpawnObject(_cannons[cannonIndex].position, _cannons[cannonIndex].rotation);
        }
    }

    private void GetAccuracy(float accuracy)
    {
        _accuracy = accuracy;
    }
    private void HomingSwitch()
    {
        _masterHoming = !_masterHoming;
        HomingButton.ToggleHoming.Invoke(_masterHoming);
    }
    private IEnumerator ReloadProjectile()
    {
        ShootButton.ReloadUIEffect?.Invoke(_reloadDuration);
        yield return _waitReloadDuration;
        _canShoot = true;
    }
    
    
}
