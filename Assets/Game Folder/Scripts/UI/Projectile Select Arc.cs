using UnityEngine;

public class ProjectileSelectArc : SelectRadialArc
{
    [SerializeField] private int _projectileIndex;
    protected override void OnClick()
    {
        CannonControl.ProjectileSelecter.Invoke(_projectileIndex);
    }
}
