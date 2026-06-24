using UnityEngine;

public class ThrottleSelectArc : SelectRadialArc
{
    [SerializeField] private int _throttleIndex;
    protected override void OnClick()
    {
        ThrottleButton.ThrottleRadialMenuStateSetter.Invoke(false);
        SpaceshipMovement.ThrottleChange.Invoke(_throttleIndex);
    }
}
