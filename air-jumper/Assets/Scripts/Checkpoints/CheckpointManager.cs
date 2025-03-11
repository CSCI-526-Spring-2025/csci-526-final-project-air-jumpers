using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    // Pending checkpoints array to store the order of checkpoints visited
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private int checkpointCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        hasCheckpoint = true;
    }

    public Vector3 GetCheckpointPosition(Vector3 defaultPosition)
    {
        return hasCheckpoint ? lastCheckpointPosition : defaultPosition;
    }

    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }

    public void incrementCheckpoint()
    {
        checkpointCount += 1;
    }

    public int getCheckpointCount()
    {
        return checkpointCount;
    }

}
