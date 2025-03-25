using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI winText;

    private float gameOverTimer = 10f;
    private bool isGameOverTimerRunning = false;
    private Coroutine countdownCoroutine;

    private SendToGoogle sendToGoogle; // SendToGoogle Object Initialization

    public void StartGameOverTimer()
    {
        //if (!isGameOverTimerRunning)
        //{
        //    isGameOverTimerRunning = true;
        //    countdownText.gameObject.SetActive(true);
        //    countdownCoroutine = StartCoroutine(GameOverCountdown());
        //}
    }

    public void CancelGameOverTimer()
    {
        if (isGameOverTimerRunning)
        {
            isGameOverTimerRunning = false;
            countdownText.gameObject.SetActive(false);
        }

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    private IEnumerator GameOverCountdown()
    {
        
        float timeRemaining = gameOverTimer;

        while (timeRemaining > 0)
        {
            countdownText.text = "Game Over in: " + Mathf.Ceil(timeRemaining).ToString() + "s";
            yield return new WaitForSeconds(1f);
            timeRemaining--;

            if (FindObjectOfType<PlayerMovement>().HasPlatforms())
            {
                CancelGameOverTimer();
                yield break;
            }
            gameOverTimer = timeRemaining;
        }

        GameOver();
    }

    private void GameOver()
    {
        Debug.Log("Game Over! Restarting...");
        GameManager.Instance.Start();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Need work to retain same sessionId if the same user restart

        // Send the analytics for the same user after game over
        sendToGoogle = FindObjectOfType<SendToGoogle>();
        sendToGoogle.Send();
    }

    public void StopTimer()
    {
        winText.gameObject.SetActive(true);

        if (isGameOverTimerRunning)
        {
            isGameOverTimerRunning = false;
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
            countdownText.gameObject.SetActive(false);
            winText.text = "You Win!";
        }
    }
}
