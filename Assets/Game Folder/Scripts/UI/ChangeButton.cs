using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButton : MonoBehaviour
{
    [Header("Change Button")]
    [SerializeField] private Transform _changeButtonTransform;
    [SerializeField] private Image _changeButtonIcon;
    [SerializeField] private Image _changeButtonArc;

    [Header("Change Radial Menu")]
    [SerializeField] private GameObject _changeRadialMenu;

    [Header("Hide Objects")]
    [SerializeField] private GameObject[] _objectsToHide;

    [Header("Scale Settings")]
    [SerializeField] private float _expandedScale = 2f;
    [SerializeField] private float _normalScale = 1f;
    [SerializeField] private float _hideScale = 0f;

    [Header("Animation Settings")]
    [SerializeField] private float _scaleAnimationDuration = 0.25f;
    [SerializeField] private float _fillAnimationDuration = 0.1f;

    [Header("Arc Fill")]
    [SerializeField] private float _fullFillAmount = 1f;
    [SerializeField] private float _emptyFillAmount = 0f;

    [Header("Image Alpha")]
    [SerializeField] private float _inactiveAlpha = 0.1f;
    [SerializeField] private float _activeAlpha = 1f;

    [Header("Tween Settings")]
    [SerializeField] private Ease _scaleEase = Ease.InOutBack;
    [SerializeField] private Ease _fillEase = Ease.InOutCubic;

    private bool _isChangeRadialMenuActive = false;
    public static Action<bool> ChangeButtonStateSetter;
    private Color _inactiveColor;
    private Color _activeColor;

    private Tween _arcTween;

    private void OnEnable()
    {
        InputManager.OnChange += OnChangeButtonPressed;
        ChangeButtonStateSetter += ChangeButtonState;
    }

    private void OnDisable()
    {
        InputManager.OnChange -= OnChangeButtonPressed;
        ChangeButtonStateSetter -= ChangeButtonState;
    }

    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
    }

    public void OnChangeButtonPressed()
    {
        if (_isChangeRadialMenuActive)
        {
            ChangeButtonState(false);
            ThrottleButton.ThrottleRadialMenuStateSetter?.Invoke(false);
        }
        else 
        {
            ChangeButtonState(true);
        }
    }


    private void ChangeButtonState(bool requiredState)
    {
        if (requiredState)
        {
            _isChangeRadialMenuActive = true;

            _changeButtonIcon.color = _inactiveColor;

            _changeButtonTransform.DOKill();

            _changeButtonTransform
                .DOScale(_expandedScale, _scaleAnimationDuration)
                .SetEase(_scaleEase)
                .OnComplete(() =>
                {
                    AnimateArc(_fullFillAmount, _emptyFillAmount);

                    _changeRadialMenu.transform.localScale = Vector3.zero;
                    _changeRadialMenu.SetActive(true);

                    _changeRadialMenu.transform.DOKill();
                    _changeRadialMenu.transform
                        .DOScale(_normalScale, _scaleAnimationDuration)
                        .SetEase(_scaleEase);

                    int countOtherObjects = _objectsToHide.Length;
                    for (int i = 0; i < countOtherObjects; i++)
                    {
                        int index = i;

                        _objectsToHide[index].transform.DOKill();
                        _objectsToHide[index].transform
                            .DOScale(_hideScale, _scaleAnimationDuration)
                            .SetEase(_scaleEase)
                            .OnComplete(() =>
                            {
                                _objectsToHide[index].SetActive(false);
                            });
                    }

                    UIEffects.SlowMotionEffectEvent?.Invoke(true);
                });
        }
        else
        {
            _isChangeRadialMenuActive = false;

            _changeButtonTransform.DOKill();
            _changeButtonTransform
                .DOScale(_normalScale, _scaleAnimationDuration)
                .SetEase(_scaleEase);

            _changeRadialMenu.transform.DOKill();
            _changeRadialMenu.transform
                .DOScale(_hideScale, _scaleAnimationDuration)
                .SetEase(_scaleEase)
                .OnComplete(() =>
                {
                    AnimateArc(_emptyFillAmount, _fullFillAmount);

                    _changeButtonIcon.color = _activeColor;

                    _changeRadialMenu.SetActive(false);

                    int countOtherObjects = _objectsToHide.Length;
                    for (int i = 0; i < countOtherObjects; i++)
                    {
                        _objectsToHide[i].SetActive(true);

                        _objectsToHide[i].transform.DOKill();
                        _objectsToHide[i].transform
                            .DOScale(_normalScale, _scaleAnimationDuration)
                            .SetEase(_scaleEase);
                    }

                    DOVirtual.DelayedCall(0.1f, () =>
                    {
                        UIEffects.SlowMotionEffectEvent?.Invoke(false);
                    });
                });
        }
    }

    private void AnimateArc(float from, float to)
    {
        _arcTween?.Kill();
        _arcTween = DOVirtual.Float(
            from, to,
            _fillAnimationDuration,
            value => _changeButtonArc.fillAmount = value)
            .SetEase(_fillEase);
    }
}
