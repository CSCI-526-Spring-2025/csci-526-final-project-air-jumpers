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
    public void RegisterCheckpoint(Vector3 checkpointPosition)
    {
        // Check if the checkpoint has already been visited
        bool alreadyVisited = visitedCheckpoints.Exists(cp => Vector3.Distance(cp.CheckpointPosition, checkpointPosition) < 0.1f);

        if (!alreadyVisited)
        {
            // Calculate the current time relative to the level start
            float currentTime = Time.time - SendToGoogle.Instance.GetLevelStartTime(); // Use SendToGoogle's level start time
            float timeSinceLastCheckpoint = 0f;

            // Calculate the time since the last checkpoint
            if (visitedCheckpoints.Count > 0)
            {
                float lastTimeReached = visitedCheckpoints[^1].TimeReached;
                timeSinceLastCheckpoint = currentTime - lastTimeReached;
            }

            // Create a new CheckpointData object and add it to the list
            var checkpoint = new CheckpointData(checkpointPosition, currentTime, timeSinceLastCheckpoint);
            visitedCheckpoints.Add(checkpoint);

            Debug.Log($"Checkpoint at {checkpointPosition} reached at {currentTime:F2}s (Δ {timeSinceLastCheckpoint:F2}s)");
        }
    }


    /// <summary>
    /// Returns the position of the last visited checkpoint.
    /// Returns Vector3.zero if no checkpoint has been visited.
    /// </summary>
    public Vector3 GetLastCheckpointPosition()
    {
        return visitedCheckpoints.Count > 0 ? visitedCheckpoints[^1].CheckpointPosition : Vector3.zero;
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
