using UnityEngine;
using System.Collections;

public abstract class ProjectileBase: MonoBehaviour
{
    protected float Thrust;
    protected float Torque;
    protected float DestroyTime;
    protected float DestroyTimer;
    protected float PostHitTime;
    protected Rigidbody Rigidbody;
    protected TrailRenderer TrailRenderer;

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
    }
    protected virtual void Movement()
    {
        Rigidbody.AddForce(transform.forward * Thrust, ForceMode.Impulse);
        Rigidbody.AddTorque(transform.forward * Torque, ForceMode.Impulse);
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
