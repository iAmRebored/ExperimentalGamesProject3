using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public float yThreshold = -10f; // The Y level that triggers game over
    public GameObject gameOverCanvas; // The UI Canvas to activate

    private bool gameOverTriggered = false;

    void Update()
    {
        if (!gameOverTriggered && transform.position.y < yThreshold)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        gameOverTriggered = true;

        // Optionally: Freeze the game
        Time.timeScale = 0f;
    }
}
