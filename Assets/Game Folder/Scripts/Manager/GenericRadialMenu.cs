using UnityEngine;

public class GenericRadialMenu : MonoBehaviour
{

    [SerializeField] private RectTransform _center;
    [SerializeField] private RectTransform[] _options;
    [SerializeField] private float _radius;
    [Range(0, 360)]
    [SerializeField] private float _totalUseAngle;
    [SerializeField] private float _padding;


    [SerializeField] private Vector2[] _cachedPositions;
    public void BakePositions()
    {
        float thetaPerOption = (_totalUseAngle / _options.Length);
        _cachedPositions = new Vector2[_options.Length];

        for (int i = 0; i < _options.Length; i++)
        {
            _cachedPositions[i] = FindPositionOfOptions(_center.position, _radius, Mathf.Deg2Rad * thetaPerOption * i);
            _options[i].position = _cachedPositions[i];
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this); // Marks scene dirty so it saves
#endif
    }

    private Vector2 FindPositionOfOptions(Vector2 center, float radius, float theta)
    {
        return new Vector2(center.x + radius * Mathf.Cos(theta), center.y + radius * Mathf.Sin(theta));
    }
}
