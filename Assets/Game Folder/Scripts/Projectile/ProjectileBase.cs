using UnityEngine;
using System.Collections;

public abstract class ProjectileBase: MonoBehaviour
{
    protected float HomingThrust;
    protected float ForwardThrust;
    protected float Torque;
    protected float DestroyTime;
    protected float DestroyTimer;
    protected float PostHitTime;
    protected Rigidbody Rigidbody;
    protected TrailRenderer TrailRenderer;

    private Transform _targetObject;
    private Vector3 _startPosition;
    private Vector3 _targetDirection;
    private bool _homingEnabled = false;


    [SerializeField] private AnimationCurve _trajectoryCurve;
    [SerializeField] private float _trajectoryArcRatio = 0.1f;
    private float _maxTrajectoryArc;
    private float _travelTime;
    private float _elapsedTime = 0f;

    protected virtual void OnEnable()
    {
        Reset(); 
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }
    protected virtual void Update()
    {
        DestroyCountdown();
    }
    private void Reset()
    {
        TrailRenderer.Clear();
        Rigidbody.linearVelocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        DestroyTimer = 0;
        _elapsedTime = 0;
        _targetObject = null;
        _homingEnabled = false;

    }
    public void InitialiseDirectionOfProjectile(Transform targetObject,bool homing)
    {
        _targetObject = targetObject;
        _homingEnabled = homing;
        _startPosition = transform.position;
        
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
            Rigidbody.AddForce(transform.forward*ForwardThrust, ForceMode.Impulse);
        }

    }
    private void TrajectoryMovement(Vector3 startPosition, Vector3 endPosition)
    {
        
        float distance = Vector3.Distance(startPosition, endPosition);
        _travelTime = distance / HomingThrust;
        _maxTrajectoryArc = distance * _trajectoryArcRatio;

        _elapsedTime += Time.fixedDeltaTime;
        float t = Mathf.Clamp01(_elapsedTime / _travelTime);

        Vector3 linearInterpolation = Vector3.Lerp(startPosition, endPosition, t);

        Vector3 travelDirection = (endPosition - startPosition).normalized;

        Vector3 arcAxis = Vector3.Cross(travelDirection, Vector3.forward).normalized;


        float nextArcPosition = _trajectoryCurve.Evaluate(t);

        float arcPosition = Mathf.Lerp(0, _maxTrajectoryArc, nextArcPosition);

        Rigidbody.MovePosition((linearInterpolation + arcAxis * arcPosition));

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnHit(collision);
    }

    protected abstract void OnHit(Collision collision);

    protected abstract void ScheduleDestroyTime(float time);
    private void DestroyCountdown()
    {
        if (DestroyTimer < DestroyTime)
        {
            DestroyTimer += Time.deltaTime;
        }
        else
        {
            ScheduleDestroyTime(PostHitTime);
        }
    }
}
