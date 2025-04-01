using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject); //Destroying bullet after hitting the player
        }
        else if (collision.CompareTag("Ground")) //Destroying bullet if it hits the ground
        {
            Destroy(gameObject);
        }
    }
}
