using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorChange : MonoBehaviour
{
    public PostProcessVolume mainPostProcessVolume;
    public PostProcessVolume reversePostProcessVolume;
    public MicrophoneHandler microphoneHandler;
    public KeyCode toggleKey = KeyCode.P;
    public float saturatedValue = 0f;
    public float desaturatedValue = -100f;
    public float transitionSpeed = 2f;

    private ColorGrading mainColorGrading;
    private ColorGrading reverseColorGrading;

    private float targetMainSaturation;
    private float targetReverseSaturation;

    public Renderer targetRenderer;

    private float targetBlend = 0f;
    private float currentBlend = 0f;

    private bool lastTriggered = false;

    void Start()
    {
        if (mainPostProcessVolume == null || reversePostProcessVolume == null)
        {
            Debug.LogError("PostProcessVolumes not assigned.");
            return;
        }

        if (!mainPostProcessVolume.profile.TryGetSettings(out mainColorGrading))
            Debug.LogError("Main ColorGrading not found.");

        if (!reversePostProcessVolume.profile.TryGetSettings(out reverseColorGrading))
            Debug.LogError("Reverse ColorGrading not found.");

        SetSaturationTargets(triggered: false, instant: true);
        targetRenderer.material.SetFloat("_Blend", 1f); // Start as greyscale
    }

    void Update()
    {
        bool triggered = Input.GetKey(toggleKey) || microphoneHandler.flipped;

        // Only update if the triggered state has changed
        if (triggered != lastTriggered)
        {
            SetSaturationTargets(triggered);
           targetBlend = triggered ? 1f : 0f; // 1 = color, 0 = greyscale
            lastTriggered = triggered;
        }

        // Smooth transition for post-processing
        if (mainColorGrading != null)
        {
            mainColorGrading.saturation.value = Mathf.Lerp(
                mainColorGrading.saturation.value,
                targetMainSaturation,
                Time.deltaTime * transitionSpeed
            );
        }

        if (reverseColorGrading != null)
        {
            reverseColorGrading.saturation.value = Mathf.Lerp(
                reverseColorGrading.saturation.value,
                targetReverseSaturation,
                Time.deltaTime * transitionSpeed
            );
        }

        // Smooth transition for material blend
        currentBlend = Mathf.Lerp(currentBlend, targetBlend, Time.deltaTime * transitionSpeed);
        targetRenderer.material.SetFloat("_Blend", currentBlend);

        // Reset mic flag
        microphoneHandler.flipped = false;
    }

    void SetSaturationTargets(bool triggered, bool instant = false)
    {
        if (triggered)
        {
            targetMainSaturation = saturatedValue;
            targetReverseSaturation = desaturatedValue;
        }
        else
        {
            targetMainSaturation = desaturatedValue;
            targetReverseSaturation = saturatedValue;
        }

        if (instant)
        {
            if (mainColorGrading != null)
                mainColorGrading.saturation.value = targetMainSaturation;

            if (reverseColorGrading != null)
                reverseColorGrading.saturation.value = targetReverseSaturation;
        }
    }
}