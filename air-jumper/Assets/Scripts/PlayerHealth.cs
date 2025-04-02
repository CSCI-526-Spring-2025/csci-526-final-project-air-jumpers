using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar UI")]
    public Slider healthBarSlider;
    public float damageCooldown = 1f;
    private float lastDamageTime;
    public Image fill;
    public Gradient gradient;

    private Rigidbody2D rb;
    public float knockbackForce = 5f;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
            fill.color = gradient.Evaluate(1.0f);
        }
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (Time.time >= lastDamageTime + damageCooldown)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
            lastDamageTime = Time.time;

            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
            fill.color = gradient.Evaluate(healthBarSlider.normalizedValue);
        }

    }

    private void Die()
    {
        Debug.Log("Player Died!");
        // FindObjectOfType<PlayerMovement>().RespawnPlayer();

        // Update the game over count in SendToGoogle
        SendToGoogle sendToGoogle = FindObjectOfType<SendToGoogle>();
        if (sendToGoogle != null)
        {
            sendToGoogle.incrementGameOverCount();
        }

        // New respawn logic has been added here
        FindObjectOfType<PlayerMovement>().Respawn();
        currentHealth = maxHealth;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(40f);
        }
        else if (collision.CompareTag("Spike"))
        {
            TakeDamage(60f);
            Knockback(collision.transform.position);
        }
        else if(collision.CompareTag("Laser"))
        {
            TakeDamage(maxHealth);
        }
    }

    private void Knockback(Vector3 hazardPosition)
    {
        if (rb != null)
        {
            Vector2 knockbackDirection = (transform.position - hazardPosition).normalized;
            rb.velocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }


    public void ResetPlayerHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
}
