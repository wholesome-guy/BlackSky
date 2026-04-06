using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody _spaceshipRigidbody;
    private InputManager _inputManager;

    [SerializeField] private float _forwardThrottle;
    [SerializeField] private float _sideThrottle;
    [SerializeField] private float _rollTorque;
    [SerializeField] private float _yawTorque;
    [SerializeField] private float _pitchTorque;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        PhysicsMovement();
    }

    private void PhysicsMovement()
    {
        Vector3 thrust = new Vector3(_inputManager.ThrottleInput.x * _sideThrottle,0f,_inputManager.ThrottleInput.y * _forwardThrottle);
        _spaceshipRigidbody.AddRelativeForce(thrust, ForceMode.Force);

        if (!_inputManager.aimBool)
        {
            Vector3 torque = new Vector3(-_inputManager.PitchYawInput.y * _pitchTorque, _inputManager.PitchYawInput.x * _yawTorque, -_inputManager.RollInput * _rollTorque);
            _spaceshipRigidbody.AddRelativeTorque(torque, ForceMode.Force);
        }

    }


}
