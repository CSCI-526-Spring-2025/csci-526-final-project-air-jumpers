using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    private float gameOverTimer = 10f;
    private bool isGameOverTimerRunning = false;
    public TextMeshProUGUI winText;

    public void StartGameOverTimer()
    {
        if (!isGameOverTimerRunning)
        {
            isGameOverTimerRunning = true;
            gameOverTimer = 10f; //Reset timer to 10 every time it starts
            countdownText.gameObject.SetActive(true); //Show timer
            InvokeRepeating("UpdateCountdownDisplay", 0f, 1f);
            Invoke("GameOver", gameOverTimer);
        }
    }

    public void CancelGameOverTimer()
    {
        if (isGameOverTimerRunning)
        {
            isGameOverTimerRunning = false;
            CancelInvoke("GameOver");
            CancelInvoke("UpdateCountdownDisplay");
            countdownText.gameObject.SetActive(false);
        }
    }

    private void UpdateCountdownDisplay()
    {
        if (isGameOverTimerRunning)
        {
            gameOverTimer -= 1f;
            countdownText.text = "Game Over in: " + Mathf.Ceil(gameOverTimer).ToString() + "s";
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Restarting...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopTimer()
    {
        winText.gameObject.SetActive(true);

        if (isGameOverTimerRunning)
        {

            isGameOverTimerRunning = false;
            CancelInvoke("GameOver");
            CancelInvoke("UpdateCountdownDisplay");
            countdownText.gameObject.SetActive(false);
            winText.text = "You Win!";

        }
    }

}
