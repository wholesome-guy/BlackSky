using UnityEngine;

public class StickingAnchor : MonoBehaviour
{
[SerializeField] private int _spaceshipAnchorIndex;
[SerializeField] private VerletRope _verletRope;
private Transform _spaceshipAnchorTransform;

    void Start()
    {
        _spaceshipAnchorTransform = SpaceshipAnchor.GetSpaceshipAnchorTransform?.Invoke(_spaceshipAnchorIndex);
        _verletRope._spaceshipAnchorTransform = _spaceshipAnchorTransform;
    }
    public int SpaceshipAnchorIndexGetter()
    {
        return _spaceshipAnchorIndex;
    }
}    

    
    


