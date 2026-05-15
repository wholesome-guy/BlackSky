using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEditor.ShaderGraph.Internal;

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

    public static Action<float> ReloadUIEffect;

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
        _shootIcon.color = new Color(1, 1, 1, _inactiveAlpha);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(DOVirtual.Float(1, 0, _bufferTime, (f) => { _shootArc.fillAmount = f; }).SetEase(Ease.InOutCubic));
        sequence.Join(transform.DOScale(_inactiveScale, _bufferTime).SetEase(Ease.InOutCubic));
        sequence.Append(DOVirtual.Float(0, 1, duration - _bufferTime, (a) => { _shootArc.fillAmount = a; }).SetEase(Ease.InOutCubic));
        sequence.Join(transform.DOScale(_activeScale, duration - _bufferTime).SetEase(Ease.InOutCubic));
        sequence.OnComplete(() => {_shootIcon.color = new Color(1,1,1,_activeAlpha); });
        
    }
}
