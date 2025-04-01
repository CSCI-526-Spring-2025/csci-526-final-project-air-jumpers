using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint; //bullet spawn
    public float fireRate = 1.5f;
    public float bulletSpeed = 5f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("Shoot", 1f, fireRate); // Shoot interval
    }

    private void Shoot()
    {
        if (player == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            //Aiming at player
            Vector2 direction = (player.position - firePoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }
}
