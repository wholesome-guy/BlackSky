using TMPro;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;

    private void OnEnable()
    {
        SpaceshipMovement.SpeedAccess += SpeedUpdating;
    }
    private void OnDisable()
    {
        SpaceshipMovement.SpeedAccess -= SpeedUpdating;
    }

    private void SpeedUpdating(float speed)
    {
        _speedText.text = speed.ToString("F0") + "m/s";
    }

}
