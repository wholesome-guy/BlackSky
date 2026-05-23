using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Instantiate(prefab,RandomPosition(transform.position),Quaternion.identity);
            Destroy(gameObject,0.5f);
        }
    }
    private Vector3 RandomPosition(Vector3 position)
    {
        float randomDistance = Random.Range(50.0f, 100.0f);
        float randomDirectionX = Random.Range(-1.0f, 1.0f);
        float randomDirectionZ = Random.Range(-1.0f, 1.0f);

        Vector3 randomDirection = new Vector3(randomDirectionX, 0, randomDirectionZ).normalized;
        Vector3 randomPosition = position + randomDirection * randomDistance;

        return randomPosition;

    }

    private void Update()
    {
        transform.Translate(0.1f, 0.1f, 0.1f);
    }
}
