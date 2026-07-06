using UnityEngine;
using System;

public class SpaceshipAnchor : MonoBehaviour
{[SerializeField] private Transform[] _spaceshipAnchorTransforms;
    public static Func<int, Transform> GetSpaceshipAnchorTransform;

    private void OnEnable()
    {
        GetSpaceshipAnchorTransform += ChooseShipAnchor;
    }

    private void OnDisable()
    {
        GetSpaceshipAnchorTransform -= ChooseShipAnchor;
    }

    private Transform ChooseShipAnchor(int index)
    {
        return _spaceshipAnchorTransforms[index];
    }

}
