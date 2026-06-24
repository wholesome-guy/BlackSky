using UnityEngine;
using UnityEngine.UI;

public class UItest : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Start()
    {
        image.alphaHitTestMinimumThreshold = 0.1f;
    }

    public void TestMethod()
    {
        Debug.Log("DID IT WORK");
    }
}
