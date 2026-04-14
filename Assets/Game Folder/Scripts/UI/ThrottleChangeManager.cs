using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ThrottleChangeManager : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _throttleRadialMenu;
    [SerializeField] private Image _throttleImage;
    [SerializeField] private Sprite[] _throttleIcons;
    [SerializeField] private Image[] _optionSelectArc;

     public void OnPointerDown(PointerEventData eventData)
     {
        transform.DOScale(2, 0.25f).SetEase(Ease.InOutBack).OnComplete
        (
            () =>
        {
            _throttleRadialMenu.transform.localScale = Vector3.zero;
            _throttleRadialMenu.SetActive(true);
            _throttleRadialMenu.transform.DOScale(1, 0.25f).SetEase(Ease.InOutBack);
            UIEffects.SlowMotionEffectEvent.Invoke(true);
        });
     }

    public void SelectThrottle(int i)
    {
        if (i > 2) return;
        SpaceshipMovement.ThrottleChange?.Invoke(i);

        _optionSelectArc[i].gameObject.SetActive(true);
        _optionSelectArc[i].fillAmount = 0;
        DOVirtual.Float(0, 1, 0.1f, (v) => { _optionSelectArc[i].fillAmount = v; }).SetEase(Ease.InOutCubic);

        transform.DOScale(1.5f, 0.25f).SetEase(Ease.InOutBack);
        _throttleRadialMenu.transform.DOScale(0, 0.25f).SetEase(Ease.InOutBack).OnComplete
            (() => 
            {
                _throttleImage.sprite = _throttleIcons[i];
                _throttleRadialMenu.SetActive(false);
                _optionSelectArc[i].gameObject.SetActive(false);
                UIEffects.SlowMotionEffectEvent.Invoke(false);
            });

    }


}
