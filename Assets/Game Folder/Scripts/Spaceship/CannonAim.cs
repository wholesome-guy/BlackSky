using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CannonAim : MonoBehaviour
{
    [SerializeField] private float _maxShootDistance = 100;
    private InputManager _inputManager;


    private void Start()
    {
        _inputManager = InputManager.Instance;
        IntialiseRotation();
    }
    private void Update()
    {
        if (_inputManager.aimBool)
        {
            RayToInput();
        }
        else
        {
            IntialiseRotation();
        }
    }
    private void RayToInput()
    {
        Vector3 targetPosition;
        RaycastHit hit;

        Ray inputRay = Camera.main.ScreenPointToRay(InputLookDirection(_inputManager.PitchYawInput));

        if (Physics.Raycast(inputRay, out hit))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = inputRay.origin + inputRay.direction * _maxShootDistance;
        }
        RotateToInput(targetPosition);
    }
    private void RotateToInput(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        transform.forward = direction;

        transform.rotation = Quaternion.LookRotation(direction);
    }
    private Vector2 InputLookDirection(Vector2 input)
    {
        float originX = 960f, originY = 540f;

        float scaleXPositive = 540f, scaleXNegative = 640f;
        float scaleYPositive = 310f, scaleYNegative = 280f;

        float screenX = input.x >= 0
            ? originX + input.x * scaleXPositive
            : originX + input.x * scaleXNegative;

        float screenY = input.y >= 0
            ? originY + input.y * scaleYPositive
            : originY + input.y * scaleYNegative;

        return new Vector2(screenX, screenY);
    }
    private void IntialiseRotation()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(InputLookDirection(Vector2.zero));

        Vector3 targetPosition = inputRay.origin + inputRay.direction * _maxShootDistance;

        RotateToInput(targetPosition);

    }

}
