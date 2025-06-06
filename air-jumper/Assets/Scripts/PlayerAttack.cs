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
    public Animator animator;

    [Header("Gun Controls")]
    public GameObject bulletPrefab;
    public bool isGunInUse = false;
    public float bulletSpeed = 10f;

    private bool isAttacking;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && Time.time >= lastAttackTime + attackCooldown)
        {
            if (!isGunInUse)
            {
                animator.SetTrigger("Attack");
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
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
    }
    public void PerformHitboxCheck()
    {
        Vector2 attackPosition = attackPoint.position;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);

        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("EnemyHitbox"))
            {
                Enemy enemyScript = col.GetComponentInParent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
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
