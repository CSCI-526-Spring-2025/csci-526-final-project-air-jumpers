using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles sending player analytics data to a backend API.
/// The data is collected during gameplay and sent in one of the following conditions:
/// 1. Player has made progress but quits the game
/// 2. Player triggers a level restart
/// 3. Player reaches the win condition
/// </summary>
public class SendAnalytics : MonoBehaviour
{
    public static SendAnalytics Instance { get; private set; }

    [SerializeField] private string endpoint = "http://localhost:8000/analytics"; // Replace with actual server URL when deployed

    private long _sessionID;
    private float _levelStartTime;
    private float _levelElapsedTime;
    private int _regularPlatformCount;
    private int _buildingPlatformCount;
    private int _visitedCheckpointsCount;
    private List<CheckpointData> _visitedCheckpoints = new List<CheckpointData>();
    private int _replayButtonClicked;
    private int _gameOverCount;
    private int _jumpCount;
    private int _currentLevel;
    private float _timeToFlag;
    private int _jumpsToFlag;
    private bool _isCurrentWin;
    private List<int> _healthAfterKills = new List<int>();

    PlayerMovement playerMovement;
    BuildingInventoryManager buildingInventoryManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _sessionID = DateTime.Now.Ticks;
        _currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("SendAnalytics initialized. Current Level: " + _currentLevel);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    /// <summary>
    /// Resets level-specific variables when a new scene is loaded.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentLevel = scene.buildIndex;
        _levelStartTime = Time.time;
        _regularPlatformCount = 0;
        _buildingPlatformCount = 0;
        _visitedCheckpointsCount = 0;
        _jumpCount = 0;
        _gameOverCount = 0;
        _visitedCheckpoints.Clear();
        _isCurrentWin = false;

        Debug.Log($"New level loaded: {_currentLevel}. Timer reset.");
    }

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        buildingInventoryManager = FindObjectOfType<BuildingInventoryManager>();
    }

    /// <summary>
    /// Formats player analytics as JSON and sends it via POST to the backend API.
    /// </summary>
    private IEnumerator Post()
    {
        _levelElapsedTime = Time.time - _levelStartTime;
        _visitedCheckpoints = newCheckpointManager.Instance.GetVisitedCheckpoints();
        _visitedCheckpointsCount = newCheckpointManager.Instance.GetCheckpointCount();

        _timeToFlag = -1;
        _jumpsToFlag = -1;

        if (_isCurrentWin)
        {
            if (_visitedCheckpoints.Count > 0)
            {
                CheckpointData lastCheckpoint = _visitedCheckpoints[^1];
                _timeToFlag = _levelElapsedTime - lastCheckpoint.TimeReached;
                _jumpsToFlag = _jumpCount - lastCheckpoint.TotalJumps;
            }
            else
            {
                _timeToFlag = _levelElapsedTime;
                _jumpsToFlag = _jumpCount;
            }
        }

        var payload = new AnalyticsPayload
        {
            session_id = _sessionID.ToString(),
            level_index = _currentLevel,
            regular_platform_count = _regularPlatformCount,
            building_platform_count = _buildingPlatformCount,
            total_jump_count = _jumpCount,
            game_over_count = _gameOverCount,
            checkpoint_data = _visitedCheckpoints,
            time_to_flag = _timeToFlag,
            jumps_to_flag = _jumpsToFlag,
            health_after_kills = _healthAfterKills
        };

        string json = JsonUtility.ToJson(payload);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        // Test output payload value
        // VisualizePayload(payload);

        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Analytics successfully sent to backend.");
        }
        else
        {
            Debug.LogError("Error sending analytics: " + request.error);
        }
    }

    /// <summary>
    /// Triggers the process to send analytics data to the backend.
    /// </summary>
    public void Send()
    {
        Debug.Log("Game Over Count (Send): " + _gameOverCount);
        StartCoroutine(Post());
    }

    /// <summary>
    /// Sends analytics if the player quits after progress but before winning.
    /// </summary>
    private void OnApplicationQuit()
    {
        if (playerMovement.getPlatformCreated() >= 4 && !_isCurrentWin)
        {
            Debug.Log("Game is quitting. Sending analytics...");
            Send();
        }
    }

    public float GetLevelStartTime() => _levelStartTime;
    public void incrementRegularPlatformCount() => _regularPlatformCount++;
    public void incrementBuildingPlatformCount() => _buildingPlatformCount++;
    public void decrementBuildingPlatformCount() => _buildingPlatformCount--;
    public void incrementGameOverCount()
    {
        _gameOverCount++;
        Debug.Log("Game Over Count: " + _gameOverCount);
    }

    public void IncrementReplayButtonClickCount() => _replayButtonClicked++;
    public void IncrementPlayerJumpCount() => _jumpCount++;
    public int GetJumpCount() => _jumpCount;
    public void SetIsCurrentWin(bool status) => _isCurrentWin = status;
    public void RecordHealthAfterKill(int currentHealth) => _healthAfterKills.Add(currentHealth);

    /// <summary>
    /// Outputs all checkpoint data to the console for debugging purposes.
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
            Debug.Log($"Checkpoint #{i + 1} | Pos: {cp.CheckpointPosition} | Time: {cp.TimeReached:F2}s | Δ From Previous: {cp.TimeSinceLastCheckpoint:F2}s | Total Jumps: {cp.TotalJumps}");
        }
        Debug.Log("===========================");
    }

    /// <summary>
/// Logs the payload content to the Unity Console for debugging.
/// </summary>
    private void VisualizePayload(AnalyticsPayload payload)
    {
        Debug.Log("=== Analytics Payload ===");
        Debug.Log($"Session ID: {payload.session_id}");
        Debug.Log($"Level Index: {payload.level_index}");
        Debug.Log($"Regular Platforms: {payload.regular_platform_count}");
        Debug.Log($"Building Platforms: {payload.building_platform_count}");
        Debug.Log($"Total Jumps: {payload.total_jump_count}");
        Debug.Log($"Game Over Count: {payload.game_over_count}");
        Debug.Log($"Time to Flag: {payload.time_to_flag}");
        Debug.Log($"Jumps to Flag: {payload.jumps_to_flag}");

        Debug.Log("Health After Kills: " + string.Join(", ", payload.health_after_kills));

        if (payload.checkpoint_data != null && payload.checkpoint_data.Count > 0)
        {
            Debug.Log("Checkpoint Data:");
            for (int i = 0; i < payload.checkpoint_data.Count; i++)
            {
                var cp = payload.checkpoint_data[i];
                Debug.Log($"Checkpoint #{i + 1} - Pos: {cp.CheckpointPosition}, TimeReached: {cp.TimeReached:F2}, TimeΔ: {cp.TimeSinceLastCheckpoint:F2}, Jumps: {cp.JumpsSinceLastCheckpoint}, Total Jumps: {cp.TotalJumps}");
            }
        }
        else
        {
            Debug.Log("Checkpoint Data: [None]");
        }

        Debug.Log("=========================");
    }

}

[Serializable]
public class AnalyticsPayload
{
    public string session_id;
    public int level_index;
    public int regular_platform_count;
    public int building_platform_count;
    public int total_jump_count;
    public int game_over_count;
    public List<CheckpointData> checkpoint_data;
    public float time_to_flag;
    public int jumps_to_flag;
    public List<int> health_after_kills;
}
