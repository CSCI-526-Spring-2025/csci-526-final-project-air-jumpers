using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles sending player analytics data to a Google Form.
/// </summary>
public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;

    private long _sessionID;
    private int _newPlatformCount;
    private int _reachedCheckpoints;
    // Start is called before the first frame update

    private int _collectiblesCount;
    private int _enemyDefeated;

    private float _levelElapsedTime;

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
    private IEnumerator Post(string sessionID, string newPlatformCount, string reachedCheckedPoints, string levelElapsedTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.595435075", sessionID);
        form.AddField("entry.220358108", newPlatformCount);
        form.AddField("entry.1107099891", reachedCheckedPoints);
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
        _reachedCheckpoints = 0;

        StartCoroutine(Post(_sessionID.ToString(), _newPlatformCount.ToString(), _reachedCheckpoints.ToString(), _levelElapsedTime.ToString("F2")));
    }

    /// <summary>
    /// Called when the application is about to quit.
    /// This ensures that final analytics data is sent before the game closes.
    /// </summary>
    private void OnApplicationQuit()
    {
        // Debug.Log(playerMovement.getPlatformCount());
        Debug.Log("Game is quitting. Sending analytics...");
        Send();
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
