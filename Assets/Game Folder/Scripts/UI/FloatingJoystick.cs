using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickKnob;

    private Vector2 _joystickPosition; 
    private float _radius;
    public Vector2 JoystickInput { get; private set; }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 joystickDirection = eventData.position - _joystickPosition;
        JoystickInput = (joystickDirection.magnitude > _radius) ? joystickDirection.normalized : joystickDirection / (_radius);
        _joystickKnob.anchoredPosition = JoystickInput * _radius;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _radius = _joystickBackground.sizeDelta.x / 2f;

        _joystickBackground.gameObject.SetActive(true);
        _joystickPosition = eventData.position;
         OnDrag(eventData);
        _joystickBackground.position = eventData.position;
        _joystickKnob.anchoredPosition = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickBackground.gameObject.SetActive(false);
        JoystickInput = Vector2.zero;
        _joystickKnob.anchoredPosition =  Vector2.zero;
    }
}
