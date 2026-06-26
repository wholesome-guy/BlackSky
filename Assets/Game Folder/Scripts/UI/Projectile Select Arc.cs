using UnityEngine;

public class ProjectileSelectArc : SelectRadialArc
{
    [SerializeField] private int _projectileIndex;
    protected override void OnClick()
    {
        ProjectileButton.ProjectileRadialMenuStateSetter.Invoke(false);
        CannonControl.ProjectileSelecter.Invoke(_projectileIndex);
    }
}
