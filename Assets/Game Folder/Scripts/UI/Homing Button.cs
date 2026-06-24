using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;


public class HomingButton : MonoBehaviour
{
    [SerializeField] private Transform _homingButtonTransform;
    [SerializeField] private Image _homingButtonIcon;
    [SerializeField] private Image _homingButtonArc;

    [Header("Tween values")]
    [SerializeField] private float _expandedScale = 1.5f;
    [SerializeField] private float _normalScale = 1.0f;
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private float _fillDuration = 0.25f;
    [SerializeField] private float _delayDuration = 0.5f;
    [SerializeField] private float _inactiveAlpha = 0.1f;
    [SerializeField] private float _activeAlpha = 1f;
    [SerializeField] private Ease _tweenEase;

    private Color _inactiveColor;
    private Color _activeColor;
    private Sequence _scaleSequence;

    public static Action<bool> ToggleHoming;
    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
    }
    private void OnEnable()
    {
        ToggleHoming += HomingSwitch;
    }
    private void OnDisable()
    {
        ToggleHoming -= HomingSwitch;
    }

    private void HomingSwitch(bool boolean)
    {
        if (boolean)
        {
            _homingButtonIcon.color = _activeColor;
            DOVirtual.Float(0f, 1f, _fillDuration, (f) => { _homingButtonArc.fillAmount = f; }).SetEase(Ease.InOutCubic);
        }
        else
        {
            _homingButtonIcon.color = _inactiveColor;
            DOVirtual.Float(1f, 0f, _fillDuration, (f) => { _homingButtonArc.fillAmount = f; }).SetEase(Ease.InOutCubic);
        }

        _scaleSequence = DOTween.Sequence();
        _scaleSequence.Append(_homingButtonTransform.DOScale(_expandedScale,_scaleDuration).SetEase(_tweenEase));
        _scaleSequence.Append(_homingButtonTransform.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase));

        DOVirtual.DelayedCall(_delayDuration, () =>
        {
            ChangeButton.ChangeButtonStateSetter?.Invoke(false);
        });
    }

}
