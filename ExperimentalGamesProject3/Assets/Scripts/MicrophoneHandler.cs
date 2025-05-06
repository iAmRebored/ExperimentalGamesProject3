using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class MicrophoneHandler : MonoBehaviour
{
    // Start is called before the first frame update

     public GameObject originalObject;
    public GameObject replacementObject;
    public int sensitivity = 2;
    private bool isChanged = false;
    public float cooldownTime = 1f;
    public float fadeSpeed = 1f;

    private AudioClip micClip;
    private string micDevice;
    private float lastDetectedTime;
    private bool fadingBack = false;
    private Material originalMaterial;
    private Color originalColor;
    void Start()
    {
         if (Microphone.devices.Length == 0){
            Debug.LogError("No microphone found!");
            return;
        }

            
    
        micDevice = Microphone.devices[0];
        micClip = Microphone.Start(micDevice, true, 1, 44100);
        originalMaterial = originalObject.GetComponent<MeshRenderer>().material;
        originalColor = originalMaterial.color;


        if (originalObject != null && replacementObject != null)
        {
            replacementObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (micClip == null) return;

        float[] samples = new float[128];
        int micPos = Microphone.GetPosition(micDevice) - samples.Length + 1;
        if (micPos < 0) return;

        micClip.GetData(samples, micPos);

        float volume = 0f;
        foreach (float sample in samples)
        {
            volume += Mathf.Abs(sample);
        }
         
        Debug.Log(volume);
        if (volume > sensitivity)
        {
            originalObject.SetActive(false);
            Debug.Log("originalObject is " + originalObject.name);
            replacementObject.SetActive(true);
            SetAlpha(originalMaterial, 1f);

            

            lastDetectedTime = Time.time;
            fadingBack = false;
        }
        else if (Time.time - lastDetectedTime > cooldownTime && !fadingBack)
        {
            fadingBack = true;

            StartCoroutine(FadeBack());
        }


        
    }

    System.Collections.IEnumerator FadeBack()
    {
        replacementObject.SetActive(false);
        originalObject.SetActive(true);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            SetAlpha(originalMaterial, t);
            yield return null;
        }

        SetAlpha(originalMaterial, 1f);
        fadingBack = false;
    }

    void SetAlpha(Material mat, float alpha)
    {
        Color c = mat.color;
        c.a = Mathf.Clamp01(alpha);
        mat.color = c;
    }

   private void SetActiveObject(bool micDetected)
    {
        originalObject.SetActive(!micDetected);
        replacementObject.SetActive(micDetected);
    }
}
