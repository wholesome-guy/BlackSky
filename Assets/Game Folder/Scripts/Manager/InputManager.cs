using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private Spaceship_Controls _spaceshipControls;

    [Header("Joystick")]
    [SerializeField] private FloatingJoystick _floatingJoystickScript;
    [SerializeField] private GameObject _floatingJoystick;
    [SerializeField] private GameObject _fixedJoystick;
    private bool _isFloatingJoystickActive = false;


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
   
    public void ChangeJoystick(bool boolean)
    {
        _isFloatingJoystickActive = boolean;
        if(_isFloatingJoystickActive)
        {
            _floatingJoystick.SetActive(true);
            _fixedJoystick.SetActive(false);
        }
        else
        {
            _floatingJoystick.SetActive(false);
            _fixedJoystick.SetActive(true);
        }
    }
    private void JoystickInput()
    {
        if (_isFloatingJoystickActive)
        {
            PitchYawRollInput = _floatingJoystickScript.JoystickInput;
        }
        else
        {
            PitchYawRollInput = _spaceshipControls.Spaceship.PitchYawRoll.ReadValue<Vector2>().normalized;
        }
    }



}
