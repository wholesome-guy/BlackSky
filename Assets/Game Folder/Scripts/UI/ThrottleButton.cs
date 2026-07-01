using DG.Tweening;
using System;
using UnityEngine;

public class ThrottleButton : RadialMenuButton
{
    
    public static Action<bool> ThrottleRadialMenuStateSetter;
    [SerializeField] private RadialMenuButton _projectileButton;

    private void OnEnable()
    {
        InputManager.OnThrottle += OnButtonPress;
        ThrottleRadialMenuStateSetter += RadialMenuState;
    }
    private void OnDisable()
    {
        InputManager.OnThrottle -= OnButtonPress;
        ThrottleRadialMenuStateSetter -= RadialMenuState;
    }

    protected override void OnButtonPress()
    {
        _scaleSequence.Kill();
        if(_projectileButton._isRadialMenuActive)
        {
            ProjectileButton.ProjectileRadialMenuStateSetter?.Invoke(false);
        }
        RadialMenuState(!_isRadialMenuActive);
    }
}
