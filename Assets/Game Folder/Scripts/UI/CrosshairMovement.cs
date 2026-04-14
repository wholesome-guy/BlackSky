using UnityEngine;

public class CrosshairMovement : MonoBehaviour
{
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private float _smoothSpeed = 8f; // Lower = floatier, Higher = snappier
    [SerializeField] private Transform _spaceship;

    private Vector2 _currentPosition;

    void Start()
    {
        _currentPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Project the ship's forward point into screen space
        Vector3 worldAimPoint = _spaceship.position + _spaceship.forward * 100f;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldAimPoint);

        // Convert screen pos to anchored position (offset from screen center)
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 targetPosition = screenPos - screenCenter;

        // Smooth it
        _currentPosition = Vector2.Lerp(_currentPosition, targetPosition, _smoothSpeed * Time.deltaTime);
        _crosshair.anchoredPosition = _currentPosition;
    }

   
}
