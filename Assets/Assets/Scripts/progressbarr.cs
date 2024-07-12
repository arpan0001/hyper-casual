using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Transform player; // The player object
    public Transform target; // The target object
    public Slider progressBar; // The UI Slider
    
    private float initialDistance; // The initial distance between player and target
    private const float maxSliderValue = 0.6f; // The maximum value for the slider

    void Start()
    {
        // Calculate the initial distance between the player and the target
        initialDistance = Vector3.Distance(player.position, target.position);
        
        // Ensure the progress bar starts empty
        progressBar.value = 0; 
        progressBar.minValue = 0;
        progressBar.maxValue = maxSliderValue;
    }

    void Update()
    {
        // Calculate the current distance between the player and the target
        float currentDistance = Vector3.Distance(player.position, target.position);

        // Calculate the progress, scale it to the range 0 to maxSliderValue, and clamp it between 0 and maxSliderValue
        float progress = Mathf.Clamp01(1 - (currentDistance / initialDistance)) * maxSliderValue;

        // Update the progress bar
        progressBar.value = progress;
    }
}
