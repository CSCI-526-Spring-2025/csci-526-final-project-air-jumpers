using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public GameObject platformPrefab;
    public TextMeshProUGUI platformCounterText;
    private Rigidbody2D rb;
    private int platformCount = 1;

    // platformCount tracks how many platforms are left to be created
    // private int platformCount = 10; 

    // plaformUsed tracks how many plaforms has been created;
    private int platformCreated = 0;
    private bool isGrounded = false;
    private bool isOnPlatform = false;
    private bool facingRight = true;

    private int jumpTimes = 1;
    private Vector3 startPosition;

    private float startTime; // Stores the game start time

    private bool isWin;

    private SendToGoogle sendToGoogle; // SendToGoogle Object Initialization


    /*
    Pending development here to store the visited checkpoint in to a
    dynamic array along with the player so that the last position store the last
    checkpoint the player visited and the length of the array will be the total
    visited checkpoint count
    */
    private List<Checkpoint> visitedCheckpoints;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        UpdatePlatformCounter();

        StartTime(); // Start the game timer
    }

    void Update()
    {
        MovePlayer();
        Jump();
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isOnPlatform && platformCount > 0)
        {
            SpawnPlatform();
        }
    }

    void MovePlayer()
    {
        float move = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            move = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move = 1f;
        }
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        if ((isGrounded || isOnPlatform || (jumpTimes > 0)) && Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimes -= 1;
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        PlayerAttack attack = GetComponent<PlayerAttack>();
        if (attack != null)
        {
            attack.FlipAttackPoint();
        }
    }


    void SpawnPlatform()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 1.2f, 0);
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        platformCount--;
        platformCreated++;
        UpdatePlatformCounter();

        if (platformCount == 0)
        {
            FindObjectOfType<GameOverManager>().StartGameOverTimer();
        }

    }

    void UpdatePlatformCounter()
    {
        if (platformCounterText != null)
        {
            platformCounterText.text = "Platforms: " + platformCount;
        }
    }

    private bool CheckIsOnUpperSideOfPlatform(Collision2D collision)
    {
        bool reached = true;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Angle(Vector2.up, contact.normal) > 45)
            {
                reached = false;
            }
        }
        return reached;
    }

    private void CheckContactWithPlayerBuiltPlatforms(Collision2D collision)
    {
        BuildingMaterialCanceler canceler = collision.gameObject.GetComponentInChildren<BuildingMaterialCanceler>();
        if (canceler != null)
        {
            canceler.cancalable = false;
            jumpTimes = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpTimes = 1;
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            if (CheckIsOnUpperSideOfPlatform(collision))
            {
                isOnPlatform = true;
                jumpTimes = 1;
                CheckContactWithPlayerBuiltPlatforms(collision);
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WinFlag"))
        {
            WinGame();
        }

        if (collision.CompareTag("PowerUp"))
        {
            Debug.Log("Triggered");

            Destroy(collision.gameObject);
            FindObjectOfType<GameOverManager>().CancelGameOverTimer();
        }
    }

    public void AddPlatform(int count)
    {
        platformCount += count;
        UpdatePlatformCounter();
        FindObjectOfType<GameOverManager>().CancelGameOverTimer();
    }

    private void WinGame()
    {
        //Stop the timer and show a win message
        FindObjectOfType<GameOverManager>().StopTimer();
        Debug.Log("You Win!");

        // Update the win status
        isWin = true;
        
        // Send the analytics for the same user after game over
        sendToGoogle = FindObjectOfType<SendToGoogle>();
        sendToGoogle.Send();
    }
    public bool HasPlatforms()
    {
        return platformCount > 0;
    }
public void RespawnPlayer()
{
    if (CheckpointManager.Instance.HasCheckpoint())
    {
        transform.position = CheckpointManager.Instance.GetCheckpointPosition(startPosition);
    }
    else
    {
        transform.position = startPosition; //No checkpoint reached yet
    }
}


    /// <summary>
    /// Gets the total number of platforms created by the player.
    /// </summary>
    /// <returns>The number of platforms the player has created.</returns>
    public int getPlatformCreated()
    {
        // Debug.Log(platformCreated); // Uncomment for debugging if needed
        return platformCreated;
    }

    /// <summary>
    /// Starts the timer by recording the current game time.
    /// This should be called when the game begins or when timing needs to be reset.
    /// </summary>
    void StartTime()
    {
        startTime = Time.time; // Store the current time as the start time
    }

    /// <summary>
    /// Retrieves the elapsed time since the timer started.
    /// </summary>
    /// <returns>The total elapsed time in seconds.</returns>
    public float getElapsedTime()
    {
        return Time.time - startTime; // Calculate elapsed time by subtracting startTime from the current time
    }

    public bool winCheck(){
        return isWin;
    }

}
