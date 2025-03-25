using System.Collections.Generic;
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
            DontDestroyOnLoad(gameObject); //Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject); //Destroy duplicate if another instance exists
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

    //New level function start

    public void ResetCheckpoints(Vector3 newGroundPosition)
    {
        lastCheckpointPosition = newGroundPosition;
        hasCheckpoint = true;  // Update the checkpoint
    }
    //New level function End


}
