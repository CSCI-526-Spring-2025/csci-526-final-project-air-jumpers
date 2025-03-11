using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public static Checkpoint Instance;

    private bool isVisited = false; // Initialize a variable to determine whether the checkpoint has been visited or not

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isVisited)
        {
            isVisited = true;
            CheckpointManager.Instance.SetCheckpoint(transform.position);
            spriteRenderer.color = Color.green; // Change color to green when activated

            CheckpointManager.Instance.incrementCheckpoint();

        }
    }

    public void ColorChange(Color colr)
    {
        spriteRenderer.color = colr;
    }
}
