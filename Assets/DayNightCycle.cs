using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight;    // The directional light in your scene
    public float dayDuration = 150f;  // Total time for one full cycle (2.5 minutes = 150 seconds)
    public Color dayColor = Color.white;   // Light color during the day
    public Color nightColor = Color.blue;  // Light color during the night
    public float maxIntensity = 1.0f;      // Maximum light intensity during the day
    public float minIntensity = 0.2f;      // Minimum light intensity during the night

    private float currentTime = 0f;

    void Update()
    {
        // Increment time based on how fast the cycle should complete
        currentTime += Time.deltaTime;

        // Loop the time back after completing one cycle
        if (currentTime >= dayDuration)
        {
            currentTime = 0f;
        }

        // Calculate the current percentage of the cycle (from 0 to 1)
        float timePercent = currentTime / dayDuration;

        // Rotate the light to simulate the sun's movement
        directionalLight.transform.rotation = Quaternion.Euler((currentTime / dayDuration * 360f) - 90f, 170f, 0f);

        // Lerp between day and night color
        directionalLight.color = Color.Lerp(nightColor, dayColor, Mathf.Clamp01((Mathf.Sin(currentTime / dayDuration * Mathf.PI * 2) + 1f) / 2f));

        // Manually force the light to refresh
        directionalLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.Clamp01((Mathf.Sin(currentTime / dayDuration * Mathf.PI * 2) + 1f) / 2f));
        
        // Call this to ensure WebGL picks up the changes
        directionalLight.enabled = false;
        directionalLight.enabled = true;



        
    }
}
