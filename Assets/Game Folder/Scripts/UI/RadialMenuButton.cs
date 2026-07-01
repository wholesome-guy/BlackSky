using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class RadialMenuButton : MonoBehaviour
{
    [SerializeField] private Transform _buttonTransform;
    [SerializeField] private Image _buttonArc;
    [SerializeField] private Image _buttonIcon;
    [SerializeField] private Transform _radialMenu;

    [Header("Tween values")]
    [SerializeField] private float _expandedScale = 1.5f;
    [SerializeField] private float _normalScale = 1.0f;
    [SerializeField] private float _hideScale = 0f;
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private float _fillDuration = 0.25f;
    [SerializeField] private float _delayDuration = 0.25f;
    [SerializeField] private float _inactiveAlpha = 0.1f;
    [SerializeField] private float _activeAlpha = 1f;
    [SerializeField] private Ease _tweenEase;
    protected Sequence _scaleSequence;

    private Color _inactiveColor;
    private Color _activeColor;

    public bool _isRadialMenuActive { get; private set; }

    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
        _scaleSequence = DOTween.Sequence();
        _isRadialMenuActive = false;
    }
    protected abstract void OnButtonPress();

    protected void RadialMenuState(bool requiredState)
    {
        _scaleSequence.Kill();
        if (requiredState)
        {
            _scaleSequence.Append(_buttonTransform.DOScale(_expandedScale, _scaleDuration).SetEase(_tweenEase));
            _scaleSequence.Join(DOVirtual.Float(0f, 1f, _fillDuration, (f) => { _buttonArc.fillAmount = f; }).SetEase(_tweenEase));
            _scaleSequence.Append(_buttonTransform.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase));
            _buttonIcon.color = _activeColor;

            _radialMenu.localScale = Vector3.zero;
            _radialMenu.gameObject.SetActive(false);
            _isRadialMenuActive = true;

            DOVirtual.DelayedCall(_delayDuration, () => {
                _radialMenu.gameObject.SetActive(true);
                _radialMenu.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase);
            });
        }
        else
        {
            _scaleSequence.Append(_buttonTransform.DOScale(_expandedScale, _scaleDuration).SetEase(_tweenEase));
            _scaleSequence.Join(DOVirtual.Float(1f, 0f, _fillDuration, (f) => { _buttonArc.fillAmount = f; }).SetEase(_tweenEase));
            _scaleSequence.Append(_buttonTransform.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase));
            _buttonIcon.color = _inactiveColor;


            _isRadialMenuActive = false;

            DOVirtual.DelayedCall(_delayDuration, () => {
                _radialMenu.DOScale(_hideScale, _scaleDuration).SetEase(_tweenEase).OnComplete(() =>
                {
                    _radialMenu.gameObject.SetActive(false);
                });
            });
            

        }
    }


}
