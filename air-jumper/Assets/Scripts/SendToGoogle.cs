using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles sending player analytics data to a Google Form.
/// The analytics will be sent in three conditions:
/// 1. When user have some progresss but exit the application
/// 2. User action casuing the game to restart
/// 3. User reached the winFlag
/// </summary>
public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;

    // Track the session ID
    private long _sessionID;

    // Track the numbers of platforms created by the player
    private int _newPlatformCount;

    // Track the number of checkpoints reached by the player
    private int _reachedCheckpoints;

    // A list stored the visited checkpoints information
    private List<string> _checkpointsVisited = new List<string>();

    // Track the numbers of replay button clicked
    private int _replayButtonClicked;

    // Track how many times the player health dropped to 0
    private int _gameOverCount;

    // Track the time taken to finish one level
    private float _levelElapsedTime;

    // Track the times W is pressed for jumping
    private int _jumpCount;

    // Track which level player is currently in
    private int _currentLevel;



    // private int _collectiblesCount;
    // private int _enemyDefeated;
    // private bool isWin;
    // private int gameOverCount;

    PlayerMovement playerMovement;

    private void Awake()
    {
        _sessionID = DateTime.Now.Ticks;

        // Send();
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
    /// Increments the number of platforms created by the player.
    /// </summary>
    public void incrementGameOverCount()
    {
        _gameOverCount++;
    }

    /// <summary>
    /// Collects player data and starts the upload process.
    /// </summary>
    public void Send()
    {
        _newPlatformCount = playerMovement.getPlatformCreated();
        _levelElapsedTime = playerMovement.getElapsedTime();
        _reachedCheckpoints = newCheckpointManager.Instance.GetCheckpointCount();

        StartCoroutine(Post(_sessionID.ToString(), _newPlatformCount.ToString(), _reachedCheckpoints.ToString(), _levelElapsedTime.ToString("F2")));
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

    /// <summary>
    /// Initializes references when the game starts.
    /// </summary>
    void Start()
    {
        // Find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
}
