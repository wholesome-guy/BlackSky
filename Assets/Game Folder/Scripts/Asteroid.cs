using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _asteroidMass;
    [SerializeField] private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody.mass = _asteroidMass;
    }
}
