using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    public float patrolRange = 5f;
    private float leftLimit, rightLimit;
    private int direction = 1;

    [Header("Health Bar")]
    public Slider healthBar;
    public Transform healthBarCanvas;
    public Vector3 healthBarOffset = new Vector3(0, 1f, 0);

    [Header("Collectible")]
    public CollectibleType collectibleType = CollectibleType.s_PlatformCollectible;

    private Vector3 startPos;

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;
        leftLimit = startPos.x - patrolRange;
        rightLimit = startPos.x + patrolRange;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (collectibleType == CollectibleType.b_BlockCollectible)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        } 
        else if (collectibleType == CollectibleType.b_GunCollectible)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void Update()
    {
        Patrol();
        UpdateHealthBar();
    }


    private void Patrol()
    {
        transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x >= rightLimit && direction == 1)
        {
            Flip();
        }
        else if (transform.position.x <= leftLimit && direction == -1)
        {
            Flip();
        }
    }

    private void Flip()
    {
        direction *= -1;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (healthBarCanvas != null)
        {

            healthBarCanvas.position = transform.position + healthBarOffset;
            healthBarCanvas.rotation = Camera.main.transform.rotation;
        }
    }

    private void Die()
    {
        CollectibleSpawner.Instance.SpawnCollectible(collectibleType, transform.position);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(30f);
            }
        }
    }

}
