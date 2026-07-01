using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileButton : RadialMenuButton
{

    public static Action<bool> ProjectileRadialMenuStateSetter;
    [SerializeField] private RadialMenuButton _throttleButton;


    private void OnEnable()
    {
        InputManager.OnProjectileChange += OnButtonPress;
        ProjectileRadialMenuStateSetter += RadialMenuState;
    }
    private void OnDisable()
    {
        InputManager.OnProjectileChange -= OnButtonPress;
        ProjectileRadialMenuStateSetter -= RadialMenuState;
    }

    protected override void OnButtonPress()
    {
        _scaleSequence.Kill();
        if(_throttleButton._isRadialMenuActive)
        {
            ThrottleButton.ThrottleRadialMenuStateSetter?.Invoke(false);
        }
        RadialMenuState(!_isRadialMenuActive);
    }

}
