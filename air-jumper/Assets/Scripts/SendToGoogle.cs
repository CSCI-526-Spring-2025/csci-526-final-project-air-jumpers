using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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

    private long _sessionID;
    private int _newPlatformCount;
    private int _reachedCheckpoints;
    private float _levelElapsedTime;

    private int _collectiblesCount;
    private int _enemyDefeated;
    private bool isWin;
    private int gameOverCount;

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
    /// Collects player data and starts the upload process.
    /// </summary>
    public void Send()
    {
        _newPlatformCount = playerMovement.getPlatformCreated();
        _levelElapsedTime = playerMovement.getElapsedTime();
        _reachedCheckpoints = CheckpointManager.Instance.getCheckpointCount();

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
