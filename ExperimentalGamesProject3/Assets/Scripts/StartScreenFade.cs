using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    private bool hasStarted = false;

    void Update()
    {
        if (!hasStarted && Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            StartCoroutine(FadeOut());
        }
    }

    System.Collections.IEnumerator FadeOut()
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // You can now safely start gameplay logic, enable controls, etc.
    }
}
