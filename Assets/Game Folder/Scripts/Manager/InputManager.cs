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
    public bool aimBool {  get; private set; }

    public static Action OnShoot;


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
        //_spaceshipControls.Spaceship.Shoot.performed += ShootInput;
        //_spaceshipControls.Spaceship.Aim.performed += AimInput;
        //_spaceshipControls.Spaceship.Aim.canceled += AimInput;
    }

   

    private void OnDisable()
    {
        _spaceshipControls.Disable();
        //_spaceshipControls.Spaceship.Shoot.performed -= ShootInput;
        //_spaceshipControls.Spaceship.Aim.performed -= AimInput;
        //_spaceshipControls.Spaceship.Aim.canceled -= AimInput;

    }



    private void Start()
    {
        aimBool = false;
    }
    private void Update()
    {
        JoystickInput();
    }

    private void ShootInput(InputAction.CallbackContext context)
    {
        OnShoot?.Invoke();
    }
    private void AimInput(InputAction.CallbackContext context)
    {
        aimBool = !aimBool;
    }
   
    private void JoystickInput()
    {
        PitchYawRollInput = _floatingJoystickScript.JoystickInput;
    }



}
