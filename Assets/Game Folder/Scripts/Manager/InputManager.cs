using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private Spaceship_Controls _spaceshipControls;

    [Header("Joystick")]
    [SerializeField] private FloatingJoystick _floatingJoystickScript;

    public Vector2 PitchYawRollInput { get; private set; }

    public static Action OnShoot;
    public static Action OnThrottleChange;


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
        _spaceshipControls.Spaceship.ThrottleChange.performed += ThrottleChangeInput;
    }

   

    private void OnDisable()
    {
        _spaceshipControls.Disable();
        _spaceshipControls.Spaceship.Shoot.performed -= ShootInput;
        _spaceshipControls.Spaceship.ThrottleChange.performed -= ThrottleChangeInput;

    }

    private void Update()
    {
        JoystickInput();
    }

    private void ShootInput(InputAction.CallbackContext context)
    {
        OnShoot?.Invoke();
    }
    private void ThrottleChangeInput(InputAction.CallbackContext context)
    {
        OnThrottleChange?.Invoke();
    }
    private void JoystickInput()
    {
        PitchYawRollInput = _floatingJoystickScript.JoystickInput;
    }



}
