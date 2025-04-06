using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles sending player analytics data to a Google Form.
/// The analytics will be sent in three conditions:
/// 1. When user have some progresss but exit the application
/// 2. User action casuing the game to restart
/// 3. User reached the winFlag
/// </summary>
public class SendToGoogle : MonoBehaviour
{
    // Singleton instance
    public static SendToGoogle Instance { get; private set; }

    [SerializeField] private string URL;

    // Track the session ID
    private long _sessionID;

    // Track the time when the level starts
    private float _levelStartTime;

    // Track the time taken to finish one level
    private float _levelElapsedTime;

    // Track the numbers of platforms created by the player
    private int _newPlatformCount;

    // Track the number of checkpoints reached by the player
    private int _visitedCheckpointsCount;

    // A list stored the visited checkpoints information
    private List<CheckpointData> _visitedCheckpoints = new List<CheckpointData>();

    // Track the numbers of replay button clicked
    private int _replayButtonClicked;

    // Track how many times the player health dropped to 0
    private int _gameOverCount;

    // Track the times W is pressed for jumping
    private int _jumpCount;

    // Track which level player is currently in
    private int _currentLevel;

    // Track the number of building platforms created by the player
    private int _buildingPlatformCount;



    // private int _collectiblesCount;
    // private int _enemyDefeated;
    // private bool isWin;
    // private int gameOverCount;

    PlayerMovement playerMovement;
    BuildingInventoryManager buildingInventoryManager;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SendToGoogle exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _sessionID = DateTime.Now.Ticks;

        // Send();
        _currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("SendToGoogle initialized. Current Level: " + _currentLevel);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 注册场景加载事件
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 取消注册场景加载事件
    }

    /// <summary>
    /// Reset scene specific data when new scene is loaded.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the current level when a new scene is loaded
        _currentLevel = scene.buildIndex;

        // Reset the level start time and other variables
        _levelStartTime = Time.time;
        _newPlatformCount = 0;
        _visitedCheckpointsCount = 0;
        _jumpCount = 0;
        _gameOverCount = 0;
        _visitedCheckpoints.Clear();

        Debug.Log($"New level loaded: {_currentLevel}. Timer reset.");
    }

    /// <summary>
    /// Initializes references when the game starts.
    /// </summary>
    void Start()
    {
        // Find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Find the BuildingInventoryManager script in the scene
        buildingInventoryManager = FindObjectOfType<BuildingInventoryManager>();

    }




    /// <summary>
    /// Sends collected player data to a Google Form using an HTTP POST request.
    /// </summary>
    /// <param name="sessionID">Unique session identifier</param>
    /// <param name="newPlatformCount">Number of platforms created</param>
    /// <param name="reachedLevel">Highest level reached</param>
    /// /// <param name="levelElapsedTime">Time to finish one level</param>
    /// <returns>Coroutine that sends data asynchronously</returns>
    private IEnumerator Post(string sessionID, string newPlatformCount, string reachedCheckpoints, string levelElapsedTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.595435075", sessionID);
        form.AddField("entry.220358108", newPlatformCount);
        form.AddField("entry.1107099891", reachedCheckpoints);
        form.AddField("entry.2002507409", levelElapsedTime);

        // Debug.Log(sessionID);


        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    /// <summary>
    /// Collects player data and starts the upload process.
    /// </summary>
    public void Send()
    {
        _newPlatformCount = playerMovement.getPlatformCreated();
        _buildingPlatformCount = buildingInventoryManager.getPlacedPlatformsCount();
        _levelElapsedTime = Time.time - _levelStartTime;
        _visitedCheckpoints = newCheckpointManager.Instance.GetVisitedCheckpoints();
        _visitedCheckpointsCount = newCheckpointManager.Instance.GetCheckpointCount();

        // For Debug Purposes
        // DebugPrintAllCheckpoints(_visitedCheckpoints);

        // StartCoroutine(Post(_sessionID.ToString(), _newPlatformCount.ToString(), _visitedCheckpointsCount.ToString(), _levelElapsedTime.ToString("F2")));
    }

    /// <summary>
    /// Send the analytics to google when player make some progress but not reached the winFlag.
    /// This ensures that final analytics data is sent before the game closes.
    /// </summary>
    private void OnApplicationQuit()
    {
        if (playerMovement.getPlatformCreated() >= 4 && !playerMovement.winCheck())
        {
            // Debug.Log(playerMovement.getPlatformCount());
            Debug.Log("Game is quitting. Sending analytics...");
            Send();
        }
    }

    public float GetLevelStartTime()
    {
        return _levelStartTime;
    }

    /// <summary>
    /// Increments the number of platforms created by the player.
    /// </summary>
    public void incrementGameOverCount()
    {
        _gameOverCount++;
    }

    /// <summary>
    /// Increments the number of platforms created by the player.
    /// </summary>
    public void IncrementReplayButtonClickCount()
    {
        _replayButtonClicked++;
    }


    /// <summary>
    /// Prints all visited checkpoints to the console for debugging purposes.
    /// </summary>
    public void DebugPrintAllCheckpoints(List<CheckpointData> visitedCheckpoints)
    {
        if (visitedCheckpoints.Count == 0)
        {
            Debug.Log("No checkpoints visited yet.");
            return;
        }

        Debug.Log("=== Visited Checkpoints ===");

        for (int i = 0; i < visitedCheckpoints.Count; i++)
        {
            CheckpointData cp = visitedCheckpoints[i];
            Debug.Log($"Checkpoint #{i + 1} | Pos: {cp.CheckpointPosition} | Time: {cp.TimeReached:F2}s | Δ From Previous: {cp.TimeSinceLastCheckpoint:F2}s");
        }

        Debug.Log("===========================");
    }

}
