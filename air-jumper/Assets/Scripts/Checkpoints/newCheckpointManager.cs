using System.Collections.Generic;
using UnityEngine;

public class newCheckpointManager : MonoBehaviour
{
    public static newCheckpointManager Instance { get; private set; }

    private List<Vector3> visitedCheckpoints = new List<Vector3>();

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

        DontDestroyOnLoad(gameObject); // Keep across scenes if necessary
    }

    /// <summary>
    /// Registers a new checkpoint if not already visited.
    /// </summary>
    public void RegisterCheckpoint(Vector3 checkpointPosition)
    {
        if (!visitedCheckpoints.Contains(checkpointPosition))
        {
            visitedCheckpoints.Add(checkpointPosition);
            Debug.Log($"Checkpoint registered: {checkpointPosition}. Total visited: {visitedCheckpoints.Count}");
        }
    }

    /// <summary>
    /// Returns the last checkpoint visited or Vector3.zero if none.
    /// </summary>
    public Vector3 GetLastCheckpoint()
    {
        return visitedCheckpoints.Count > 0 ? visitedCheckpoints[^1] : Vector3.zero;
    }

    /// <summary>
    /// Returns the total number of visited checkpoints.
    /// </summary>
    public int GetCheckpointCount()
    {
        return visitedCheckpoints.Count;
    }
}
