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
    public static Action OnChange;
    public static Action OnHoming;
    public static Action OnThrottle;
    public static Action OnProjectileChange;


    private bool _canShoot = true;
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
        _spaceshipControls.Spaceship.Change.performed += ChangeInput;
        _spaceshipControls.Spaceship.Homing.performed += HomingSwitch;
        _spaceshipControls.Spaceship.Throttle.performed += ThrottleInput;
        _spaceshipControls.Spaceship.ProjectileChange.performed += ProjectileSelector;

    }



    private void OnDisable()
    {
        _spaceshipControls.Disable();
        _spaceshipControls.Spaceship.Shoot.performed -= ShootInput;
        _spaceshipControls.Spaceship.Change.performed -= ChangeInput;
        _spaceshipControls.Spaceship.Homing.performed -= HomingSwitch;
        _spaceshipControls.Spaceship.Throttle.performed -= ThrottleInput;
        _spaceshipControls.Spaceship.ProjectileChange.performed -= ProjectileSelector;

    }

    private void Update()
    {
        JoystickInput();
    }

    private void ShootInput(InputAction.CallbackContext context)
    {
        if (!_canShoot) return;
        OnShoot?.Invoke();
    }
    private void ChangeInput(InputAction.CallbackContext context)
    {
        OnChange?.Invoke();
        _canShoot = !_canShoot;
    }
    private void JoystickInput()
    {
        PitchYawRollInput = _floatingJoystickScript.JoystickInput;
    }
    private void HomingSwitch(InputAction.CallbackContext context)
    {
        OnHoming?.Invoke();
    }

    private void ThrottleInput(InputAction.CallbackContext context)
    {
        OnThrottle?.Invoke();
    }

    private void ProjectileSelector(InputAction.CallbackContext context)
    {
        OnProjectileChange?.Invoke();
    }

}
