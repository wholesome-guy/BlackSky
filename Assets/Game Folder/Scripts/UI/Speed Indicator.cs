using TMPro;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private float _speedMultiplier = 19;

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
        float multipliedSpeed = _speedMultiplier * speed;
        _speedText.text = multipliedSpeed.ToString("F0") + "m/s";
    }

}
