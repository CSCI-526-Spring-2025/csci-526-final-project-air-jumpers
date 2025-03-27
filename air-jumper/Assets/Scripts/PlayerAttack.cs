using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Vector2 attackSize = new Vector2(2f, 1f); 
    public float attackCooldown = 0.5f;
    public int attackDamage = 40; 
    private float lastAttackTime;

    public Transform attackPoint;
    public LayerMask enemyLayer;
    private bool facingRight = true;
    public ParticleSystem attackEffect;

    [Header("Gun Controls")]
    public GameObject bulletPrefab;
    public bool isGunInUse = false;
    public float bulletSpeed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Time.time >= lastAttackTime + attackCooldown)
        {
            if (!isGunInUse)
            {
                Attack();
            }
            else
            {
                GunAttack();
            }
        }
    }

    private void GunAttack()
    {
        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float direction = 1;
            PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                if (!playerMovement.IsFacingRight())
                {
                    direction = -1;
                }
            }
                
            rb.velocity = new Vector2(direction * bulletSpeed, 0);
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        if (attackEffect != null)
        {
            attackEffect.Play();
        }

        Vector2 attackPosition = attackPoint.position;
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
            }
        }
    }

    public void FlipAttackPoint()
    {
        facingRight = !facingRight;

        Vector3 attackPos = attackPoint.localPosition;
        attackPos.x *= -1; 
        attackPoint.localPosition = attackPos;
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Vector2 attackPosition = attackPoint.position;
            if (transform.localScale.x < 0)
            {
                attackPosition.x -= attackSize.x;
            }

            Gizmos.DrawWireCube(attackPosition, attackSize);
        }
    }
}
