using UnityEngine;
using TMPro;
using System.Collections;

public class LaserButton : MonoBehaviour
{
    [Header("Laser & Time Setting")]
    public LaserController targetLaser;
    public float disableDuration = 30f;

    [Header("UI CountDown")]
    public TextMeshProUGUI countdownText;

    // private bool isUsed = false;
    private Coroutine countdownCoroutine;

    private void Start()
    {
        //hide text at first
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (!isUsed && collision.CompareTag("Player"))
        //     StartCoroutine(DisableLaserTemporarily());
        if (collision.CompareTag("Player"))
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }

            countdownCoroutine = StartCoroutine(DisableLaserTemporarily());
        }
    }

    private IEnumerator DisableLaserTemporarily()
    {
        // isUsed = true;

        if (targetLaser != null)
            targetLaser.TurnOff();

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        float remaining = disableDuration;
        while (remaining > 0f)
        {
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(remaining).ToString();

            yield return null;
            remaining -= Time.deltaTime;
        }

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (targetLaser != null)
            targetLaser.TurnOn();

        // isUsed = false;
        countdownCoroutine = null;
    }
}

