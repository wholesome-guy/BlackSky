using UnityEngine;
using System.Collections;

public abstract class ProjectileBase: MonoBehaviour
{
    [Header("Thrust and Torque")]
    [SerializeField] private float _homingThrust = 1000;
    [SerializeField] private float _forwardThrust = 10;
    [SerializeField] private float _torqueForce = 1000;

    [Header("Timers")]
    [SerializeField] private float _destroyTime = 2f;
    [SerializeField] protected float _postHitDestroyTime = 0.05f;
    protected float _explosionDestroyTime = 10f;
    protected float _destroyTimer;
    protected bool destroyScheduled = false;

    [Header("Components")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer _trailRenderer;

    [Header("Homing")]
    private Transform _targetObject;
    private bool _homingEnabled = false;

    [Header("Trajectory Controls")]
    [SerializeField] private AnimationCurve _trajectoryCurve;
    [SerializeField] private float _trajectoryArcRatio = 0.1f;

    private float _maxTrajectoryArc;
    private float _travelTime;
    private float _elapsedTime = 0f;

    private Vector3 _cachedStartPosition;
    private Vector3 _cachedEndPosition;
    private Vector3 _cachedArcAxis;
    private const float _retargetThreshold = 0.25f;

    protected virtual void OnEnable()
    {
        ResetProjectile(); 
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }
    protected virtual void Update()
    {
        DestroyCountdown();
    }
    private void ResetProjectile()
    {
        _trailRenderer.Clear();
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _destroyTimer = 0;
        _elapsedTime = 0;
        _targetObject = null;
        _homingEnabled = false;
        destroyScheduled = false;
    }
    public void InitialiseDirectionOfProjectile(Transform targetObject,bool homing)
    {
        _targetObject = targetObject;
        _homingEnabled = homing;        
    }
    protected virtual void Movement()
    {
        if (_homingEnabled)
        {
            if (_targetObject == null) return;
            TrajectoryMovement(transform.position, _targetObject.position);
        }
        else
        {
            _rigidbody.AddForce(transform.forward*_forwardThrust, ForceMode.Impulse);
        }

    }
    private void TrajectoryMovement(Vector3 startPosition, Vector3 endPosition)
    {
        if (Vector3.SqrMagnitude(endPosition - _cachedEndPosition) > _retargetThreshold * _retargetThreshold)
        {
            float distance = Vector3.Distance(startPosition, endPosition);
            _travelTime = distance / _homingThrust;
            _maxTrajectoryArc = distance * _trajectoryArcRatio;
            _cachedEndPosition = endPosition;
            _cachedStartPosition = startPosition;

            Vector3 travelDirection = (endPosition - startPosition).normalized;
            Vector3 arcAxis = Vector3.Cross(travelDirection, transform.forward);
            _cachedArcAxis = arcAxis.normalized;
        }

        _elapsedTime += Time.fixedDeltaTime;
        float t = Mathf.Clamp01(_elapsedTime / _travelTime);

        Vector3 linearInterpolation = Vector3.Lerp(_cachedStartPosition, _cachedEndPosition, t);
        float arcPosition = Mathf.Lerp(0f, _maxTrajectoryArc, _trajectoryCurve.Evaluate(t));

        Vector3 finalPosition = linearInterpolation + _cachedArcAxis * arcPosition;

        Vector3 moveDirection = (finalPosition - _rigidbody.position).normalized;

        _rigidbody.MovePosition(finalPosition);
        //Rigidbody.MoveRotation(Quaternion.LookRotation(moveDirection));

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnHit(collision);
    }

    protected abstract void OnHit(Collision collision);

    protected abstract void ScheduleDestroyTime(float time);

    private void DestroyCountdown()
    {
        if(destroyScheduled) return;

        if (_destroyTimer < _destroyTime)
        {
            _destroyTimer += Time.deltaTime;
        }
        else
        {
            destroyScheduled = true;
            ScheduleDestroyTime(_postHitDestroyTime);
        }
    }
}
