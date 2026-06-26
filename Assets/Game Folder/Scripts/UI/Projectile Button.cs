using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileButton : MonoBehaviour
{
    [SerializeField] private Transform _projectileButtonTransform;
    [SerializeField] private Image _projectileButtonArc;
    [SerializeField] private Image _projectileButtonIcon;
    [SerializeField] private Transform _projectileRadialMenu;

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

    private bool _isProjectileRadialMenuActive = false;

    private Sequence _scaleSequence;
    private Color _inactiveColor;
    private Color _activeColor;
    private float _scaleDurationRefactor;


    public static Action<bool> ProjectileRadialMenuStateSetter;
    private void Awake()
    {
        _inactiveColor = new Color(1f, 1f, 1f, _inactiveAlpha);
        _activeColor = new Color(1f, 1f, 1f, _activeAlpha);
        _scaleDurationRefactor = _scaleDuration / 2;
    }
    private void OnEnable()
    {
        InputManager.OnProjectileChange += OnProjectileButtonPressed;
        ProjectileRadialMenuStateSetter += ProjectileRadialMenuState;
    }
    private void OnDisable()
    {
        InputManager.OnProjectileChange -= OnProjectileButtonPressed;
        ProjectileRadialMenuStateSetter -= ProjectileRadialMenuState;
    }

    private void OnProjectileButtonPressed()
    {

        _scaleSequence.Kill();
        _scaleSequence = DOTween.Sequence();


        ProjectileRadialMenuState(!_isProjectileRadialMenuActive);

    }

    private void ProjectileRadialMenuState(bool requiredState)
    {
        if (requiredState)
        {
            _scaleSequence.Append(_projectileButtonTransform.DOScale(_expandedScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _scaleSequence.Join(DOVirtual.Float(0f, 1f, _fillDuration, (f) => { _projectileButtonArc.fillAmount = f; }).SetEase(_tweenEase));
            _scaleSequence.Append(_projectileButtonTransform.DOScale(_normalScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _projectileButtonIcon.color = _activeColor;

            _projectileRadialMenu.localScale = Vector3.zero;
            _projectileRadialMenu.gameObject.SetActive(false);
            _isProjectileRadialMenuActive = true;

            _projectileRadialMenu.gameObject.SetActive(true);
            _projectileRadialMenu.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase);
        }
        else
        {
            _scaleSequence.Append(_projectileButtonTransform.DOScale(_expandedScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _scaleSequence.Join(DOVirtual.Float(1f, 0f, _fillDuration, (f) => { _projectileButtonArc.fillAmount = f; }).SetEase(_tweenEase));
            _scaleSequence.Append(_projectileButtonTransform.DOScale(_normalScale, _scaleDurationRefactor).SetEase(_tweenEase));
            _projectileButtonIcon.color = _inactiveColor;


            _isProjectileRadialMenuActive = false;
            _projectileRadialMenu.DOScale(_hideScale, _scaleDuration).SetEase(_tweenEase).OnComplete(() =>
            {
                _projectileRadialMenu.gameObject.SetActive(false);
            });

        }
    }

}
