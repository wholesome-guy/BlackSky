using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = RandomPosition(transform.position);
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
}
