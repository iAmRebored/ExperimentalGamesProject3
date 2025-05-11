using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneHandler : MonoBehaviour
{
    public List<GameObject> originalList;
    public List<GameObject> flipList;
    

    public int sensitivity = 2;
    public float cooldownTime = 1f;
    public float fadeSpeed = 1f;

    private AudioClip micClip;
    private string micDevice;
    private float lastDetectedTime;
    private bool fadingBack = false;

    public bool flipped = true;
    private List<Material> originalMaterials = new List<Material>();

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone found!");
            return;
        }

        micDevice = Microphone.devices[0];
        micClip = Microphone.Start(micDevice, true, 1, 44100);

        // Cache original materials
        foreach (GameObject obj in originalList)
        {
            if (obj.TryGetComponent(out MeshRenderer renderer))
            {
                originalMaterials.Add(renderer.material);
            }
            else
            {
                originalMaterials.Add(null);
            }
        }

        // Ensure flipList objects are disabled
        for (int i = 0; i < flipList.Count; i++)
        {
            if (flipList[i] != null)
                flipList[i].SetActive(false);
        }
    }

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

        if (volume > sensitivity)
        {
            
            SwapToFlip();
            lastDetectedTime = Time.time;
            if (fadingBack) StopAllCoroutines();
            fadingBack = false;
            flipped = true;
        }
        else if (Time.time - lastDetectedTime > cooldownTime && !fadingBack)
        {
            
            fadingBack = true;
            
            StartCoroutine(FadeBackToOriginal());
            
        }else{
            flipped = false;
        }
    }

    void SwapToFlip()
    {
        for (int i = 0; i < originalList.Count; i++)
        {
            if (originalList[i] != null) originalList[i].SetActive(false);
            if (flipList[i] != null) flipList[i].SetActive(true);
        }
    }

    IEnumerator FadeBackToOriginal()
    {
        for (int i = 0; i < flipList.Count; i++)
        {
            if (flipList[i] != null)
                flipList[i].SetActive(false);
        }

        for (int i = 0; i < originalList.Count; i++)
        {
            if (originalList[i] != null)
                originalList[i].SetActive(true);
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            for (int i = 0; i < originalMaterials.Count; i++)
            {
                if (originalMaterials[i] != null)
                    SetAlpha(originalMaterials[i], t);
            }
            yield return null;
        }

        fadingBack = false;
    
    }

    void SetAlpha(Material mat, float alpha)
    {
        Color c = mat.color;
        c.a = Mathf.Clamp01(alpha);
        mat.color = c;
    }
}
