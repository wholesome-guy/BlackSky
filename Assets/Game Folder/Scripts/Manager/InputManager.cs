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

    public Vector2 PitchYawRollInput { get; private set; }

    public bool aimBool = false;
    public bool JoystickReleased { get; private set; }

    public static Action OnShoot;

    private void Start()
    {
        JoystickReleased = false;
    }
    private void Update()
    {
        PitchYawRollInput = _spaceshipControls.Spaceship.PitchYawRoll.ReadValue<Vector2>().normalized;

        if(PitchYawRollInput.sqrMagnitude <= 0)
        {
            JoystickReleased = true;
        }
        else
        {
            JoystickReleased = false;
        }
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
