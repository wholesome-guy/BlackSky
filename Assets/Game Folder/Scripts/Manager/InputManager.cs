using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private Spaceship_Controls _spaceshipControls;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _spaceshipControls = new Spaceship_Controls();
    }
    private void OnEnable()
    {
        _spaceshipControls.Enable();
        _spaceshipControls.Spaceship.Shoot.performed += ShootInput;
        _spaceshipControls.Spaceship.Aim.performed += AimInput;
        _spaceshipControls.Spaceship.Aim.canceled += AimInput;
    }

   

    private void OnDisable()
    {
        _spaceshipControls.Disable();
        _spaceshipControls.Spaceship.Shoot.performed -= ShootInput;
        _spaceshipControls.Spaceship.Aim.performed -= AimInput;
        _spaceshipControls.Spaceship.Aim.canceled -= AimInput;

    }

    public Vector2 ThrottleInput { get; private set; }
    public Vector2 PitchYawInput { get; private set; }
    public float RollInput { get; private set; }

    public bool aimBool = false;

    public static Action OnShoot;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        ThrottleInput = _spaceshipControls.Spaceship.Throttle.ReadValue<Vector2>().normalized;

        PitchYawInput = _spaceshipControls.Spaceship.Pitch_Yaw.ReadValue<Vector2>().normalized;

        RollInput = _spaceshipControls.Spaceship.Roll.ReadValue<float>();
    }

    private void ShootInput(InputAction.CallbackContext context)
    {
        OnShoot?.Invoke();
    }
    private void AimInput(InputAction.CallbackContext context)
    {
        aimBool = !aimBool;
    }





}
