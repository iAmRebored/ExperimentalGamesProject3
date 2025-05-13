using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneBehavior : MonoBehaviour
{
    public float duration = 5f;         // How long to wait in the scene
    public string returnSceneName = "Scene01";  // Scene to load afterward
    public bool conditionMet = false;    // Set this from another script or during cutscene
    public MicrophoneHandler microphoneHandler;
    public GameObject goodGameCanvas;
    public GameObject gameOverCanvas;
    void Start()
    {
        StartCoroutine(WaitAndCheck());

    }

    IEnumerator WaitAndCheck()
    {
        yield return new WaitForSeconds(duration);

        if (microphoneHandler.flipped)
        {
            goodGameCanvas.SetActive(true);
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene(returnSceneName);
        }
        else
        {
            gameOverCanvas.SetActive(true);
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene(returnSceneName);
        }
    }
}
