using TMPro;
using UnityEngine;

public class SpeedIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;
    // Display speed is scaled up for game feel — raw physics speed is intentionally low.
    // Multiplier inflates the number to match the visual sense of velocity from VFX.
    [SerializeField] private float _speedMultiplier = 19;

    private System.Text.StringBuilder _sb = new System.Text.StringBuilder(16);


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
        _sb.Clear();
        _sb.Append((int)multipliedSpeed);  
        _sb.Append("m/s");

        _speedText.SetText(_sb);
    }

}
