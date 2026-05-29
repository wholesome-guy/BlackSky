using System;
using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField] private RectTransform _crosshair;

    [Header("Crosshair Movement Controls")]
    [SerializeField] private float _smoothSpeed = 8f;

    private Transform _spaceship;
    private Vector2 _screenCenter;
    private Camera _mainCamera;

    private Vector2 _currentPosition = Vector2.zero;
    public static Action<Vector2> CrosshairPositionAccessor;

    private void OnEnable()
    {
        SpaceshipMovement.TransformAccess += TransformAccess;
    }
    private void OnDisable()
    {
        SpaceshipMovement.TransformAccess -= TransformAccess;

    }
    private void Start()
    {
        _screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        _mainCamera = Camera.main;
    }
    void Update()
    {
        if (_spaceship == null) return;

        CrosshairPointAtSpaceshipForward();

        CrosshairPositionAccessor?.Invoke(_crosshair.position);

    }
    private void CrosshairPointAtSpaceshipForward()
    {
        Vector3 worldAimPoint = _spaceship.position + _spaceship.forward * 100f;
        Vector2 screenPosition = _mainCamera.WorldToScreenPoint(worldAimPoint);

        Vector2 targetPosition = screenPosition - _screenCenter;

        _currentPosition = Vector2.Lerp(_currentPosition, targetPosition, _smoothSpeed * Time.deltaTime);

        _crosshair.anchoredPosition = _currentPosition;
    }
    private void TransformAccess(Transform t)
    {
        //Got Player Tranform for tracking
        _spaceship = t;
    }


}
