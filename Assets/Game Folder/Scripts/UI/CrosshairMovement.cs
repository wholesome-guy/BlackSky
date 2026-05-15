using System;
using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private float _smoothSpeed = 8f;
    [SerializeField] private Transform _spaceship;

    private Vector2 _currentPosition;
    public static Action<Vector2> CrosshairPositionAccessor;

    private void OnEnable()
    {
        SpaceshipMovement.TransformAccess += TransformAccess;
    }
    private void OnDisable()
    {
        SpaceshipMovement.TransformAccess -= TransformAccess;

    }
    void Start()
    {
        _currentPosition = Vector2.zero;
    }

    void Update()
    {
        Vector3 worldAimPoint = _spaceship.position + _spaceship.forward * 100f;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldAimPoint);

        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 targetPosition = screenPos - screenCenter;

        _currentPosition = Vector2.Lerp(_currentPosition, targetPosition, _smoothSpeed * Time.deltaTime);
        _crosshair.anchoredPosition = _currentPosition;

        CrosshairPositionAccessor.Invoke(_crosshair.position);

    }
    private void TransformAccess(Transform t)
    {
        _spaceship = t;
    }


}
