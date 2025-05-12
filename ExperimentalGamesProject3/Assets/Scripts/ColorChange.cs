using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorChange : MonoBehaviour
{
    public PostProcessVolume mainPostProcessVolume;
    public PostProcessVolume reversePPVolume;
    public MicrophoneHandler microphoneHandler;
    public KeyCode toggleKey = KeyCode.P;
    public float saturatedValue = 0f; // Normal saturation value
    public float desaturatedValue = -100f; // Desaturated value
    public float transitionSpeed = 2f;

    private ColorGrading mainColorGrading;
    private ColorGrading reverseColorGrading;
    private float targetMainSaturation;
    private float targetReverseSaturation;
    private bool isDesaturated = false;

    void Start()
    {
        if (mainPostProcessVolume == null || reversePPVolume == null)
        {
            Debug.LogError("PostProcessVolumes not assigned.");
            return;
        }

        if (!mainPostProcessVolume.profile.TryGetSettings(out mainColorGrading))
        {
            Debug.LogError("ColorGrading not found in Main PostProcessVolume.");
        }

        if (!reversePPVolume.profile.TryGetSettings(out reverseColorGrading))
        {
            Debug.LogError("ColorGrading not found in ReversePP Volume.");
        }

        UpdateTargets();
    }

    void Update()
    {
        // Check for toggle key press or microphone flipped state
        if ((Input.GetKeyDown(toggleKey) || microphoneHandler.flipped) && mainColorGrading != null)
        {
            isDesaturated = !isDesaturated; // Toggle the saturation state
            UpdateTargets();

            microphoneHandler.flipped = false; // Consume the flip trigger
        }

        // Smooth transition for main camera saturation
        if (mainColorGrading != null)
        {
            mainColorGrading.saturation.value = Mathf.Lerp(
                mainColorGrading.saturation.value,
                targetMainSaturation,
                Time.deltaTime * transitionSpeed
            );
        }

        // Smooth transition for reverse camera saturation
        if (reverseColorGrading != null)
        {
            reverseColorGrading.saturation.value = Mathf.Lerp(
                reverseColorGrading.saturation.value,
                targetReverseSaturation,
                Time.deltaTime * transitionSpeed
            );
        }
    }

    // Update target saturation values based on the current state
    void UpdateTargets()
    {
        // When the main camera is desaturated, reverse camera should be fully saturated (saturatedValue)
        // When the main camera is fully saturated, reverse camera should be desaturated (desaturatedValue)
        targetMainSaturation = isDesaturated ? saturatedValue : desaturatedValue;
        targetReverseSaturation = isDesaturated ? desaturatedValue : saturatedValue;
    }
}