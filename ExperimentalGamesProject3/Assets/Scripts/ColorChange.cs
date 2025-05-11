using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColorChange : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;

    public MicrophoneHandler microphoneHandler;
    public KeyCode toggleKey = KeyCode.P;
    public float saturatedValue = 0f;
    public float desaturatedValue = -100f;
    public float transitionSpeed = 2f; // Higher = faster transition

    private ColorGrading colorGrading;
    private float targetSaturation;
    private bool isDesaturated = false;

    void Start()
    {
        if (postProcessVolume == null)
        {
            Debug.LogError("PostProcessVolume not assigned.");
            return;
        }

        if (postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            colorGrading.saturation.value = saturatedValue;
            targetSaturation = saturatedValue;
        }
        else
        {
            Debug.LogError("Color Grading not found in Post Process Volume.");
        }

        isDesaturated = !isDesaturated;
        targetSaturation = isDesaturated ? desaturatedValue : saturatedValue;
    }

    void Update()
    {
        if ((Input.GetKeyDown(toggleKey) || microphoneHandler.flipped) && colorGrading != null)
        {
            isDesaturated = false;
            targetSaturation = isDesaturated ? desaturatedValue : saturatedValue;
        }else{
            isDesaturated =true;
            targetSaturation = isDesaturated ? desaturatedValue : saturatedValue;
        }

        if (colorGrading != null)
        {
            colorGrading.saturation.value = Mathf.Lerp(
                colorGrading.saturation.value,
                targetSaturation,
                Time.deltaTime * transitionSpeed
            );
        }
    }
}