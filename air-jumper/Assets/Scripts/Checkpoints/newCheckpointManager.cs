using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages checkpoints visited by the player, recording their position and time.
/// </summary>
public class newCheckpointManager : MonoBehaviour
{
    public static newCheckpointManager Instance { get; private set; }

    // List of all visited checkpoints, including position and time reached
    private List<CheckpointData> visitedCheckpoints = new List<CheckpointData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Keep this object across scene loads (if needed)
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Registers a checkpoint when the player reaches it.
    /// </summary>
    /// <param name="checkpointPosition">The position of the checkpoint.</param>
    /// <returns>True if the checkpoint was registered, false if it was already visited.</returns>
    public void RegisterCheckpoint(Vector3 checkpointPosition)
{
    bool alreadyVisited = visitedCheckpoints.Exists(cp => cp.position == checkpointPosition);

    if (!alreadyVisited)
    {
        float currentTime = Time.time;
        float timeSinceLast = 0f;

        if (visitedCheckpoints.Count > 0)
        {
            float lastTime = visitedCheckpoints[^1].timeReached;
            timeSinceLast = currentTime - lastTime;
        }

        var checkpoint = new CheckpointData(checkpointPosition, currentTime, timeSinceLast);
        visitedCheckpoints.Add(checkpoint);

        Debug.Log($"Checkpoint at {checkpointPosition} reached at {currentTime} (Δ {timeSinceLast} sec)");
    }
}


    /// <summary>
    /// Returns the position of the last visited checkpoint.
    /// Returns Vector3.zero if no checkpoint has been visited.
    /// </summary>
    public Vector3 GetLastCheckpoint()
    {
        return visitedCheckpoints.Count > 0 ? visitedCheckpoints[^1].position : Vector3.zero;
    }

    /// <summary>
    /// Returns the number of checkpoints the player has visited.
    /// </summary>
    public List<CheckpointData> GetVisitedCheckpoints()
    {
        return visitedCheckpoints;
    }

    /// <summary>
    /// Returns the number of checkpoints the player has visited.
    /// </summary>
    public int GetCheckpointCount()
    {
        return visitedCheckpoints.Count;
    }

    /// <summary>
    /// Returns the full data (position and time) of the last visited checkpoint.
    /// Returns null if no checkpoint has been visited.
    /// </summary>
    public CheckpointData GetLastCheckpointData()
    {
        return visitedCheckpoints.Count > 0 ? visitedCheckpoints[^1] : null;
    }
}
