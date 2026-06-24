using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ThrottleChangeManager : MonoBehaviour
{
    [SerializeField] private GameObject _throttleRadialMenu;
    [SerializeField] private Image _throttleImage;
    [SerializeField] private Image _throttleArc;
    [SerializeField] private Sprite[] _throttleIcons;
    [SerializeField] private Image[] _optionSelectArc;
    [SerializeField] private GameObject[] _otherUIGameobjects;

    [Header("Scale Settings")]
    [SerializeField] private float _expandedScale = 2f;
    [SerializeField] private float _selectedScale = 1.5f;
    [SerializeField] private float _normalScale = 1f;
    [SerializeField] private float _hideScale = 0f;

    [Header("Animation Settings")]
    [SerializeField] private float _scaleAnimationDuration = 0.25f;
    [SerializeField] private float _fillAnimationDuration = 0.1f;

    [Header("Throttle Arc Fill")]
    [SerializeField] private float _fullFillAmount = 1f;
    [SerializeField] private float _emptyFillAmount = 0f;

    [Header("Throttle Image Alpha")]
    [SerializeField] private float _inactiveAlpha = 0.1f;
    [SerializeField] private float _activeAlpha = 1f;

    [Header("Throttle Settings")]
    [SerializeField] private int _maxThrottleIndex = 3;

    [Header("Tween Settings")]
    [SerializeField] private Ease _scaleEase = Ease.InOutBack;
    [SerializeField] private Ease _fillEase = Ease.InOutCubic;

    private Color _inactiveColor;
    private Color _activeColor;

    private Tween _arcTween;

    private bool isThrottleRadialActive = false;

   

    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
        _maxThrottleIndex = _optionSelectArc.Length;
    }

    public void OnThrottleChangePressed()
    {
        if (isThrottleRadialActive)
            return;

        isThrottleRadialActive = true;
        _throttleImage.color = _inactiveColor;

        transform.DOKill();

        transform
            .DOScale(_expandedScale, _scaleAnimationDuration)
            .SetEase(_scaleEase)
            .OnComplete(() =>
            {
                AnimateThrottleArc(_fullFillAmount, _emptyFillAmount);

                _throttleRadialMenu.transform.localScale = Vector3.zero;
                _throttleRadialMenu.SetActive(true);

                _throttleRadialMenu.transform.DOKill();
                _throttleRadialMenu.transform
                    .DOScale(_normalScale, _scaleAnimationDuration)
                    .SetEase(_scaleEase);

                int countOtherObjects = _otherUIGameobjects.Length;
                for (int i = 0; i < countOtherObjects; i++)
                {
                    int index = i;

                    _otherUIGameobjects[index].transform.DOKill();
                    _otherUIGameobjects[index].transform
                        .DOScale(_hideScale, _scaleAnimationDuration)
                        .SetEase(_scaleEase)
                        .OnComplete(() =>
                        {
                            _otherUIGameobjects[index].SetActive(false);
                        });
                }

                UIEffects.SlowMotionEffectEvent?.Invoke(true);
            });

    }

    public void SelectThrottle(int index)
    {
        if (index > _maxThrottleIndex|| index < 0)
            return;

        SpaceshipMovement.ThrottleChange?.Invoke(index);

        _optionSelectArc[index].gameObject.SetActive(true);
        _optionSelectArc[index].fillAmount = _emptyFillAmount;

        DOVirtual.Float(
            _emptyFillAmount,
            _fullFillAmount,
            _fillAnimationDuration,
            value => _optionSelectArc[index].fillAmount = value)
            .SetEase(_fillEase);

        transform.DOKill();
        transform
            .DOScale(_selectedScale, _scaleAnimationDuration)
            .SetEase(_scaleEase);

        _throttleRadialMenu.transform.DOKill();
        _throttleRadialMenu.transform
            .DOScale(_hideScale, _scaleAnimationDuration)
            .SetEase(_scaleEase)
            .OnComplete(() =>
            {
                AnimateThrottleArc(_emptyFillAmount, _fullFillAmount);

                _throttleImage.sprite = _throttleIcons[index];
                _throttleImage.color = _activeColor;

                _throttleRadialMenu.SetActive(false);
                _optionSelectArc[index].gameObject.SetActive(false);

                for (int i = 0; i < _otherUIGameobjects.Length; i++)
                {
                    _otherUIGameobjects[i].SetActive(true);

                    _otherUIGameobjects[i].transform.DOKill();
                    _otherUIGameobjects[i].transform
                        .DOScale(_normalScale, _scaleAnimationDuration)
                        .SetEase(_scaleEase);
                }

                DOVirtual.DelayedCall(0.1f, () =>
                {
                    UIEffects.SlowMotionEffectEvent?.Invoke(false);
                });
            });

        isThrottleRadialActive = false;
    }

    private void AnimateThrottleArc(float from, float to)
    {
        _arcTween?.Kill();
        _arcTween = DOVirtual.Float(
            from, to,
            _fillAnimationDuration,
            value => _throttleArc.fillAmount = value)
            .SetEase(_fillEase);
    }
}



