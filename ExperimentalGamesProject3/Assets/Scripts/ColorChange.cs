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
    }

    void Update()
    {
        bool triggered = Input.GetKey(toggleKey) || microphoneHandler.flipped;

        SetSaturationTargets(triggered);

        // Smooth transitions
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

        // Reset mic flag
        microphoneHandler.flipped = false;
    }

    void SetSaturationTargets(bool triggered, bool instant = false)
    {
        targetMainSaturation = triggered ? saturatedValue : desaturatedValue;
        targetReverseSaturation = triggered ? desaturatedValue : saturatedValue;

        if (instant)
        {
            if (mainColorGrading != null)
                mainColorGrading.saturation.value = targetMainSaturation;

            if (reverseColorGrading != null)
                reverseColorGrading.saturation.value = targetReverseSaturation;
        }
    }
}