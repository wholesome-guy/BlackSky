using UnityEngine;

public class ThrottleSelectArc : SelectRadialArc
{
    [SerializeField] private int _throttleIndex;
    protected override void OnClick()
    {
        SpaceshipMovement.ThrottleChange.Invoke(_throttleIndex);
    }
}
