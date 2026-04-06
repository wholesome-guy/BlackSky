using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject Cube;


    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(Cube, new Vector3(0, 0, i * 50), Quaternion.identity);
            Instantiate(Cube, new Vector3(100, 0, i * 50), Quaternion.identity);

        }
    } 
}
