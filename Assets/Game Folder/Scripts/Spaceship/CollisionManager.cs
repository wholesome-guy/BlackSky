using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _spaceshipMeshRenderers;
    [SerializeField] private Material _damageMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        MaterialChangeVFX.MaterialFlashEvent?.Invoke(_spaceshipMeshRenderers,_damageMaterial, 1, 0.25f);
    }
}
