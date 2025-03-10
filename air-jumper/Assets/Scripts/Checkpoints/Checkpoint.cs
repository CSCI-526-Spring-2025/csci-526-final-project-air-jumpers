using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckpoint(transform.position);
            spriteRenderer.color = Color.green; // Change color to green when activated
        }
    }
}
