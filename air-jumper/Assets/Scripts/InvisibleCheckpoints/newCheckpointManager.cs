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
            float currentTime = Time.time - SendToGoogle.Instance.GetLevelStartTime();
            float timeSinceLastCheckpoint = 0f;
            int jumpsSinceLastCheckpoint = 0;

            // Calculate the time and jumps since the last checkpoint
            if (visitedCheckpoints.Count > 0)
            {
                // For subsequent checkpoints
                CheckpointData lastCheckpoint = visitedCheckpoints[^1];
                timeSinceLastCheckpoint = currentTime - lastCheckpoint.TimeReached;
                jumpsSinceLastCheckpoint = SendToGoogle.Instance.GetJumpCount() - lastCheckpoint.TotalJumps;
            }
            else
            {
                // For the first checkpoint
                timeSinceLastCheckpoint = currentTime; // Time since level start
                jumpsSinceLastCheckpoint = SendToGoogle.Instance.GetJumpCount(); // Total jumps since level start
            }

            // Get the total number of jumps from PlayerMovement
            int totalJumps = SendToGoogle.Instance.GetJumpCount();

            // Create a new CheckpointData object and add it to the list
            var checkpoint = new CheckpointData(
                checkpointPosition,
                currentTime,
                timeSinceLastCheckpoint,
                totalJumps,
                jumpsSinceLastCheckpoint
            );
            visitedCheckpoints.Add(checkpoint);

            Debug.Log($"Checkpoint at {checkpointPosition} reached at {currentTime:F2}s (Δ {timeSinceLastCheckpoint:F2}s, Total Jumps: {totalJumps}, Jumps Since Last Checkpoint: {jumpsSinceLastCheckpoint})");
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
