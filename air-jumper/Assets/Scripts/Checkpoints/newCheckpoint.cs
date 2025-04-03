using UnityEngine;

/// <summary>
/// Handles checkpoint behavior when the player collides with it.
/// This script changes the checkpoint color when activated and registers it in the checkpoint manager.
/// </summary>
public class newCheckpoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isVisited = false;

    /// <summary>
    /// Initializes the checkpoint by getting the SpriteRenderer component.
    /// </summary
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
        
    }


    /// <summary>
    /// Detects when the player enters the checkpoint area.
    /// If the player collides and the checkpoint is not already activated, it registers the checkpoint.
    /// </summary>
    /// <param name="other">The Collider2D of the object that enters the checkpoint's trigger area.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isVisited)
        {
            isVisited = true;
            newCheckpointManager.Instance.RegisterCheckpoint(transform.position);
        }
    }

    /// <summary>
    /// Draws a wireframe cube in the Scene view to visualize the checkpoint trigger area.
    /// Only visible in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Vector3 gizmoPos = transform.position + (Vector3)box.offset;
            Gizmos.DrawWireCube(gizmoPos, box.size);
        }
    }

}
