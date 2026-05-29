using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class ShootButton : MonoBehaviour
{
    [SerializeField] private Image _shootIcon;
    [SerializeField] private Image _shootArc;

    [SerializeField] private float _bufferTime = 0.5f;
    [Header("Tween values")]
    [SerializeField] private float _inactiveAlpha = 0.1f;
    [SerializeField] private float _activeAlpha = 1;
    [SerializeField] private float _inactiveScale = 0.5f;
    [SerializeField] private float _activeScale = 1;

    private Color _inactiveColor;
    private Color _activeColor;
    private Sequence _reloadSequence;

    public static Action<float> ReloadUIEffect;
    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
    }
    private void OnEnable()
    {
        ReloadUIEffect += ReloadEffect;
    }
    private void OnDisable()
    {
        ReloadUIEffect -= ReloadEffect;
    }

    private void ReloadEffect(float duration)
    {
        _reloadSequence?.Kill();
        _shootIcon.color = _inactiveColor;

        float fillDuration = duration - _bufferTime;

        _reloadSequence = DOTween.Sequence();
        _reloadSequence.Append(DOVirtual.Float(1f, 0f, _bufferTime, (f) => { _shootArc.fillAmount = f; }).SetEase(Ease.InOutCubic));
        _reloadSequence.Join(transform.DOScale(_inactiveScale, _bufferTime).SetEase(Ease.InOutCubic));
        _reloadSequence.Append(DOVirtual.Float(0f, 1f, fillDuration, (a) => { _shootArc.fillAmount = a; }).SetEase(Ease.InOutCubic));
        _reloadSequence.Join(transform.DOScale(_activeScale, fillDuration).SetEase(Ease.InOutCubic));
        _reloadSequence.OnComplete(() => {_shootIcon.color = _activeColor; });
        
    }
}
