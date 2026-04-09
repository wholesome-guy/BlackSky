using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickKnob;
    private Vector2 _joystickPosition;
    public Vector2 JoystickInput { get; private set; }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 joystickDirection = eventData.position - _joystickPosition;
        JoystickInput = (joystickDirection.magnitude > _joystickBackground.sizeDelta.x/2f)? joystickDirection.normalized : joystickDirection/(_joystickBackground.sizeDelta.x / 2f);
        _joystickKnob.anchoredPosition = JoystickInput * _joystickBackground.sizeDelta.x / 2;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _joystickBackground.gameObject.SetActive(true);
        OnDrag(eventData);
        _joystickPosition = eventData.position;
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
