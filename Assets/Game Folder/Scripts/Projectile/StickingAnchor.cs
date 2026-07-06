using UnityEngine;

public class StickingAnchor : MonoBehaviour
{
[SerializeField] private int _spaceshipAnchorIndex;
[SerializeField] private Transform _spaceshipAnchorTransform;

    void Start()
    {
        _spaceshipAnchorTransform = SpaceshipAnchor.GetSpaceshipAnchorTransform?.Invoke(_spaceshipAnchorIndex);
    }
    public int SpaceshipAnchorIndexGetter()
    {
        return _spaceshipAnchorIndex;
    }
}    

    
    


