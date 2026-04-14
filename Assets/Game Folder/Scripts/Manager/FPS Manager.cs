using TMPro;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Assign this in the Inspector
    public float updateInterval = 0.5f; // How often to update the FPS display

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval


    void Start()
    {
        timeleft = updateInterval;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // Calculate the FPS
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            fpsText.text = format;

            if (fps < 30)
                fpsText.color = Color.yellow;
            else
                if (fps < 10)
                fpsText.color = Color.red;
            else
                fpsText.color = Color.green;

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }

    }


}
