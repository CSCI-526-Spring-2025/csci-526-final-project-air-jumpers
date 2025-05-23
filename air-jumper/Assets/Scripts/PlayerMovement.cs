using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public GameObject platformPrefab;
    public int initPlatformCount = 1;

    private Rigidbody2D rb;
    private int platformCount = 1;
    public GameObject winText;
    // platformCount tracks how many platforms are left to be created
    // private int platformCount = 10; 

    // plaformUsed tracks how many plaforms has been created;
    private int platformCreated = 0;
    private bool isGrounded = false;
    private bool isOnPlatform = false;
    private bool facingRight = true;

    private int jumpTimes = 1;
    private bool canSecondJump = false;
    private bool canDash = false;
    private float dashInterval = 0.2f;
    private float lastTapTimeA = 0;
    private float lastTapTimeD = 0;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth;


    /*
    Pending development here to store the visited checkpoint in to a
    dynamic array along with the player so that the last position store the last
    checkpoint the player visited and the length of the array will be the total
    visited checkpoint count
    */
    private List<Checkpoint> visitedCheckpoints;

    private List<Action> currentPlatformEffects = new List<Action>();
    private bool isJumping;

    void Start()
    {
        platformCount = initPlatformCount;

        rb = GetComponent<Rigidbody2D>();
        UpdatePlatformCounter();

        // StartTime(); // Start the game timer
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        MovePlayer();
        Jump();
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isOnPlatform && platformCount > 0)
        {
            SpawnPlatform();
            animator.Play("Player_Platform");
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        isJumping = !(isGrounded || isOnPlatform);
        animator.SetBool("IsJumping", isJumping);
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

        if (canDash)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastTapTimeA <= dashInterval)
                {
                    moveSpeed = 5 * 150;
                }
                lastTapTimeA = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastTapTimeD <= dashInterval)
                {
                    moveSpeed = 5 * 150;
                }
                lastTapTimeD = Time.time;
            }
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
            bool isDoubleJump = !isGrounded && !isOnPlatform && jumpTimes == 1;
            if (isDoubleJump)
            {
                animator?.SetTrigger("DoubleJump");
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimes -= 1;

            // Update the player jump count
            if (SendToGoogle.Instance != null)
            {
                SendToGoogle.Instance.IncrementPlayerJumpCount();
            }

            if (SendAnalytics.Instance != null)
            {
                SendAnalytics.Instance.IncrementPlayerJumpCount();
            }


            if (jumpTimes == 0)
            {
                canSecondJump = false;
            }
        }
    }

    void Flip()
    {
        
        facingRight = !facingRight;
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !facingRight;
        }
        PlayerAttack attack = GetComponent<PlayerAttack>();
        if (attack != null)
        {
            attack.FlipAttackPoint();
        }
    }


    void SpawnPlatform()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 1.2f, 0);
        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        BuildingInventoryManager.Instance.PlacePlatform(platform);

        platformCount--;
        platformCreated++;
        UpdatePlatformCounter();

        if (SendToGoogle.Instance != null)
        {
            SendToGoogle.Instance.incrementRegularPlatformCount();
        }

        if (SendAnalytics.Instance != null)
        {
            SendAnalytics.Instance.incrementRegularPlatformCount();
        }

        if (platformCount == 0)
        {
            FindObjectOfType<GameOverManager>().StartGameOverTimer();
        }
    }

    void UpdatePlatformCounter()
    {
        FindObjectOfType<PlatformIcon>()?.SetPlatformCount(platformCount);
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

    private void RegisterPlatformEffect(CollectibleType collectibleType)
    {
        switch (collectibleType)
        {
            case CollectibleType.b_BlockCollectible:
                jumpTimes = 2;
                canSecondJump = true;
                break;

            case CollectibleType.b_GunCollectible:
                PlayerAttack playerAttack = gameObject.GetComponent<PlayerAttack>();
                playerAttack.isGunInUse = true;

                currentPlatformEffects.Add(() =>
                {
                    playerAttack.isGunInUse = false;
                });
                break;

            case CollectibleType.b_DashCollectible:
                canDash = true;

                currentPlatformEffects.Add(() =>
                {
                    canDash = false;
                    moveSpeed = 5f;
                });
                break;

            default:
                break;
        }
    }

    private void ClearPlatformEffects()
    {
        foreach (Action platformEffectCleaner in currentPlatformEffects)
        {
            platformEffectCleaner();
        }

        currentPlatformEffects.Clear();
    }

    private void CheckContactWithPlayerBuiltPlatforms(Collision2D collision)
    {
        BuildingPlatformManager buildingPlatformManager = collision.gameObject.GetComponentInChildren<BuildingPlatformManager>();
        if (buildingPlatformManager != null)
        {
            buildingPlatformManager.cancalable = false;
            RegisterPlatformEffect(buildingPlatformManager.materialData.buildingType);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (CheckIsOnUpperSideOfPlatform(collision))
            {
                isGrounded = true;
                jumpTimes = 1;
            }

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
            ClearPlatformEffects();
        }

        if (!canSecondJump)
        {
            jumpTimes = 0;
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

        if(collision.CompareTag("Health"))
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            playerHealth.currentHealth=Math.Min(playerHealth.currentHealth+25, playerHealth.maxHealth);
            GameObject healthBar = GameObject.Find("HealthCollectible");
            GameObject.Destroy(healthBar);
            
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

        if (SendToGoogle.Instance != null)
        {
            // Update the win status
            SendToGoogle.Instance.SetIsCurrentWin(true);
            // Send the analytics for the same user after game over
            SendToGoogle.Instance.Send();
        }

        if (SendAnalytics.Instance != null)
        {
            // Update the win status
            SendAnalytics.Instance.SetIsCurrentWin(true);
            // Send the analytics for the same user after game over
            SendAnalytics.Instance.Send();
        }
        GameObject finalEndObject = GameObject.Find("Finalend"); 
        Sprite finalEndWinSprite = Resources.Load<Sprite>("Finalend_win"); 

        if (finalEndObject != null && finalEndWinSprite != null)
        {
            SpriteRenderer sr = finalEndObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = finalEndWinSprite;
            }
        }
        else
        {
            Debug.LogWarning("Finalend or Finalend_win not found!");
        }

        //Advance to the next level
        LoadNextSceneAsync();
    }

    public void LoadNextSceneAsync()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(nextSceneIndex);
        Debug.Log(SceneManager.sceneCountInBuildSettings);
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("load next scene");
            StartCoroutine(LoadSceneAsync(nextSceneIndex));
        }
        else
        {
            Debug.Log("No more scenes to load");
            winText.gameObject.SetActive(true);
        }

    }

    System.Collections.IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null; // Wait until the scene is fully loaded
        }
    }

    public bool HasPlatforms()
    {
        return platformCount > 0;
    }
    // public void RespawnPlayer()
    // {
    //     if (CheckpointManager.Instance.HasCheckpoint())
    //     {
    //         transform.position = CheckpointManager.Instance.GetCheckpointPosition(startPosition);
    //     }
    //     else
    //     {
    //         transform.position = startPosition; //No checkpoint reached yet
    //     }
    // }

    /// <summary>
    /// Respawns the player at the last checkpoint or the default starting position.
    /// </summary>
    public void Respawn()
    {
        if (newCheckpointManager.Instance.GetCheckpointCount() > 0)
        {
            transform.position = newCheckpointManager.Instance.GetLastCheckpointPosition();
            Debug.Log("Respawned at: " + transform.position);
        }
        else
        {

            Debug.Log("No checkpoint found. Respawn at default position.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    // /// <summary>
    // /// Starts the timer by recording the current game time.
    // /// This should be called when the game begins or when timing needs to be reset.
    // /// </summary>
    // void StartTime()
    // {
    //     startTime = Time.time; // Store the current time as the start time
    // }

    // public float getStartTime()
    // {
    //     return startTime;
    // }

    // /// <summary>
    // /// Retrieves the elapsed time since the timer started.
    // /// </summary>
    // /// <returns>The total elapsed time in seconds.</returns>
    // public float getElapsedTime()
    // {
    //     return Time.time - startTime; // Calculate elapsed time by subtracting startTime from the current time
    // }

    public bool IsFacingRight()
    {
        return facingRight;
    }
}
