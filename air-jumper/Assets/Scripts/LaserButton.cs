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

    [Header("Button Sprites")]
    public SpriteRenderer buttonSpriteRenderer; // ⭐ 新加的
    public Sprite normalSprite; // ⭐ 按钮未按下的图
    public Sprite pressedSprite; // ⭐ 按钮被按下的图

    private Coroutine countdownCoroutine;

    private void Start()
    {
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (buttonSpriteRenderer != null && normalSprite != null)
            buttonSpriteRenderer.sprite = normalSprite; // ⭐ 初始显示正常的Sprite
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }

            // ⭐ 切换到按下的Sprite
            if (buttonSpriteRenderer != null && pressedSprite != null)
                buttonSpriteRenderer.sprite = pressedSprite;

            countdownCoroutine = StartCoroutine(DisableLaserTemporarily());
        }
    }

    private IEnumerator DisableLaserTemporarily()
    {
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

        // ⭐ 倒计时结束，切回正常Sprite（可选，如果你想让按钮恢复外观）
        if (buttonSpriteRenderer != null && normalSprite != null)
            buttonSpriteRenderer.sprite = normalSprite;

        countdownCoroutine = null;
    }
}
