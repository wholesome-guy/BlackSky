using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
public class ThrottleButton : MonoBehaviour
{
    [SerializeField] private Transform _throttleButtonTransform;
    [SerializeField] private Image _throttleButtonArc;
    [SerializeField] private Image _throttleButtonIcon;
    [SerializeField] private Transform _throttleRadialMenu;

    [Header("Tween values")]
    [SerializeField] private float _expandedScale = 1.5f;
    [SerializeField] private float _normalScale = 1.0f;
    [SerializeField] private float _hideScale = 0f;
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private float _fillDuration = 0.25f;
    [SerializeField] private float _delayDuration = 0.5f;
    [SerializeField] private float _inactiveAlpha = 0.1f;
    [SerializeField] private float _activeAlpha = 1f;
    [SerializeField] private Ease _tweenEase;

    private bool _isThrottleRadialMenuActive = false;

    private Sequence _scaleSequence; 
    private Color _inactiveColor;
    private Color _activeColor;
    private float _scaleDurationRefactor;


    public static Action<bool> ThrottleRadialMenuStateSetter;
    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
        _scaleDurationRefactor = _scaleDuration / 2;
    }
    private void OnEnable()
    {
        InputManager.OnThrottle += OnThrottleButtonPressed;
        ThrottleRadialMenuStateSetter += ThrottleRadialMenuState;
    }
    private void OnDisable()
    {
        InputManager.OnThrottle -= OnThrottleButtonPressed;
        ThrottleRadialMenuStateSetter -= ThrottleRadialMenuState;
    }

    private void OnThrottleButtonPressed()
    {

        _scaleSequence.Kill();
        _scaleSequence = DOTween.Sequence();
        
        
        ThrottleRadialMenuState(!_isThrottleRadialMenuActive);

    }

    private void ThrottleRadialMenuState(bool requiredState)
    {
        if (requiredState)
        {
            _scaleSequence.Append(_throttleButtonTransform.DOScale(_expandedScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _scaleSequence.Join(DOVirtual.Float(0f, 1f, _fillDuration, (f) => { _throttleButtonArc.fillAmount = f; }).SetEase(_tweenEase));
            _scaleSequence.Append(_throttleButtonTransform.DOScale(_normalScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _throttleButtonIcon.color = _activeColor;

            _throttleRadialMenu.localScale = Vector3.zero;
            _throttleRadialMenu.gameObject.SetActive(false);
            _isThrottleRadialMenuActive = true;

            _throttleRadialMenu.gameObject.SetActive(true);
            _throttleRadialMenu.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase);
        }
        else
        {
            _scaleSequence.Append(_throttleButtonTransform.DOScale(_expandedScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _scaleSequence.Join(DOVirtual.Float(1f, 0f, _fillDuration, (f) => { _throttleButtonArc.fillAmount = f; }).SetEase(_tweenEase));
            _scaleSequence.Append(_throttleButtonTransform.DOScale(_normalScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _throttleButtonIcon.color = _inactiveColor;


            _isThrottleRadialMenuActive = false;
            _throttleRadialMenu.DOScale(_hideScale, _scaleDuration).SetEase(_tweenEase).OnComplete(() =>
            {
                _throttleRadialMenu.gameObject.SetActive(false);
            });

        }
    }

}
