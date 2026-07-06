using UnityEngine;
using System;

public class SpaceshipAnchor : MonoBehaviour
{
    [SerializeField] private Transform[] _spaceshipAnchorTransforms;
    public static Action<Transform> SpaceshipAnchorTransformAccess;
    public static Action<int> SelectSpaceshipAnchor;

    private void OnEnable()
    {
        SelectSpaceshipAnchor += ChooseShipAnchor;
    }
    private void OnDisable()
    {
        SelectSpaceshipAnchor -= ChooseShipAnchor;
    }

    private void ChooseShipAnchor(int index)
    {
        SpaceshipAnchorTransformAccess?.Invoke(_spaceshipAnchorTransforms[index]);
    }

}
