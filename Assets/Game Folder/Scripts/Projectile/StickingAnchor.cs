using UnityEngine;

public class StickingAnchor : MonoBehaviour
{
[SerializeField] private int _spaceshipAnchorIndex;
private Transform _spaceshipAnchorTransform;

    private void OnEnable()
    {
        SpaceshipAnchor.SpaceshipAnchorTransformAccess += SpaceshipAnchorTransformAccess;
    }

    private void OnDisable()
    {
        SpaceshipAnchor.SpaceshipAnchorTransformAccess -= SpaceshipAnchorTransformAccess;
    }

    void Start()
    {
        SpaceshipAnchor.SelectSpaceshipAnchor?.Invoke(_spaceshipAnchorIndex);
    }
    public void AnchorIndexSetter(int index)
    {
        _spaceshipAnchorIndex = index;
    }
    private void SpaceshipAnchorTransformAccess(Transform anchorTransform)
    {
        _spaceshipAnchorTransform = anchorTransform;
    }

}
