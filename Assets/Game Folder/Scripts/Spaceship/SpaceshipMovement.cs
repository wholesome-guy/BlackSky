using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SpaceshipMovement : MonoBehaviour
{

    [Header("Spaceship Body")]
    [SerializeField] private Rigidbody _spaceshipRigidbody;
    [SerializeField] private Transform _spaceshipMesh;
    [SerializeField] private Transform[] _spaceshipModules;

    private int _spaceshipModulesCount;

    [Header("Throttle")]
    [SerializeField] private float _lowThrottle;
    [SerializeField] private float _moderateThrottle;
    [SerializeField] private float _highThrottle;
    private float _throttle;
    [Header("Pitch Yaw Roll")]
    [SerializeField] private float _yawTorque;
    [SerializeField] private float _pitchTorque;
    [SerializeField] private float _rollClamp;
    [SerializeField] private float _rollSmoothteningValue = 2f;

    private Vector2 _rotationalInput;
    private InputManager _inputManager;



    private float _speed;
    public static Action<int> ThrottleChange;
    public static Action<float> SpeedAccess;
    public static Action<Transform> TransformAccess;

    private void OnEnable()
    {
        ThrottleChange += SelectThrottle;
    }
    private void OnDisable()
    {
        ThrottleChange -= SelectThrottle;
    }


    void Start()
    {
        _inputManager = InputManager.Instance;

        _throttle = 0;

        _spaceshipModulesCount =  _spaceshipModules.Length;

        //Access to player transform for calculations
        TransformAccess?.Invoke(transform);
    }

    private void FixedUpdate()
    {
        RotationalMovement();
        LinearMovement();
        SpaceshipSpeed();
    }

    #region Linear Movement
    private void LinearMovement()
    {
        _spaceshipRigidbody.AddForce(_throttle * transform.forward, ForceMode.Acceleration);
    }
    #endregion

    #region Rotational Movement
    private void RotationalMovement()
    {
        _rotationalInput = _inputManager.PitchYawRollInput;
        Roll();
        Yaw();
        Pitch();
    }


    private void Roll()
    {
        float targetRoll = -_rotationalInput.x * _rollClamp;

        Vector3 currentEuler = _spaceshipMesh.localEulerAngles;

        float smoothedRoll = Mathf.LerpAngle(currentEuler.z, targetRoll, _rollSmoothteningValue * Time.fixedDeltaTime);

        _spaceshipMesh.localEulerAngles = new Vector3(0f, 0f, smoothedRoll);

        for(int i = 0; i < _spaceshipModulesCount; i++ )
        {
            _spaceshipModules[i].SetPositionAndRotation(_spaceshipMesh.position, _spaceshipMesh.rotation);
        }
    }

    private void Yaw()
    {
        if (Mathf.Abs(_inputManager.PitchYawRollInput.x) > 0.1f)
        {
            _spaceshipRigidbody.AddTorque(_rotationalInput.x * _yawTorque * transform.up, ForceMode.Acceleration);
        }
        
    }
    private void Pitch()
    {
        if(Mathf.Abs(_inputManager.PitchYawRollInput.y)> 0.1f)
        {
            _spaceshipRigidbody.AddTorque(_rotationalInput.y * _pitchTorque * -1 * transform.right, ForceMode.Acceleration);
        }
        
    }

    #endregion

    private void SelectThrottle(int i)
    {
        switch (i)
        {
            case 0:
                _throttle = 0f;
                break;
            case 1:
                _throttle = _lowThrottle;
                break;
            case 2:
                _throttle = _moderateThrottle;
                break;
            case 3:
                _throttle = _highThrottle;
                break;
        }
    }

    private void SpaceshipSpeed()
    {
        _speed = _spaceshipRigidbody.linearVelocity.magnitude;
        SpeedAccess?.Invoke(_speed);
    }

}
