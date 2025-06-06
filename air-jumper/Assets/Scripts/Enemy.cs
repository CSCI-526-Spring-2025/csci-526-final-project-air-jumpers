using UnityEditor;
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
    public int collectibleNum = 3;

    private Vector3 startPos;

    public Transform spriteRoot;

    public Animator animator;

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

        ModifyColor();
    }

    private void ModifyColor()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            return;
        }

        switch (collectibleType)
        {
            case CollectibleType.b_BlockCollectible:
                renderer.color = Color.green;
                break;
            case CollectibleType.b_GunCollectible:
                renderer.color = Color.red;
                break;
            case CollectibleType.b_DashCollectible:
                renderer.color = Color.blue;
                break;
            default:
                break;
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
        if (spriteRoot != null)
        {
            Vector3 scale = spriteRoot.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            spriteRoot.localScale = scale;
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
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
        CollectibleSpawner.Instance.SpawnCollectible(collectibleType, transform.position, collectibleNum);
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            if (SendToGoogle.Instance != null)
            {
                SendToGoogle.Instance.RecordHealthAfterKill(playerHealth.GetPlayerHealth());
            }

            if (SendAnalytics.Instance != null)
            {
                SendAnalytics.Instance.RecordHealthAfterKill(playerHealth.GetPlayerHealth());
            }
        }

        Destroy(gameObject);
    }


}
