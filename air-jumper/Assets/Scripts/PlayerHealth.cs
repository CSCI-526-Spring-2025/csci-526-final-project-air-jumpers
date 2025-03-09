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



    void Start()
    {
        currentHealth = maxHealth;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

}
