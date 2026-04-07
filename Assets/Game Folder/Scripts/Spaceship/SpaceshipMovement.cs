using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody _spaceshipRigidbody;
    [SerializeField] private Transform _spaceshipMesh;
    private InputManager _inputManager;

    [SerializeField] private float _maxThrottle;
    [SerializeField] private float _maxYawTorque;
    [SerializeField] private float _maxPitchTorque;

    [SerializeField] private float _rollClamp;




    private float _throttle;
    private float _yaw;
    private float _pitch;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputManager = InputManager.Instance;
        _yaw = _maxYawTorque;
        _pitch = _maxPitchTorque;
    }

    private void FixedUpdate()
    {
        RotationalMovement();
    }

    private void LinearMovement()
    {
        
    }

    private void RotationalMovement()
    {
        Roll();
        Yaw();
        Pitch();
    }


    private void Roll()
    {
        float targetRoll = -_inputManager.PitchYawRollInput.x * _rollClamp;
        Vector3 currentEuler = _spaceshipMesh.localEulerAngles;
        float smoothedRoll = Mathf.LerpAngle(currentEuler.z, targetRoll, 2f * Time.fixedDeltaTime);
        _spaceshipMesh.localEulerAngles = new Vector3(0f, 0f, smoothedRoll);
    }

    private void Yaw()
    {
        if (Mathf.Abs(_inputManager.PitchYawRollInput.x) > 0.1f)
        {
            _spaceshipRigidbody.AddTorque(_inputManager.PitchYawRollInput.x * _yaw * transform.up, ForceMode.Acceleration);
        }
        
    }
    private void Pitch()
    {
        if(Mathf.Abs(_inputManager.PitchYawRollInput.y)> 0.1f)
        {
            _spaceshipRigidbody.AddTorque(_inputManager.PitchYawRollInput.y * _pitch * -1 * transform.right, ForceMode.Acceleration);
        }
        
    }

}
