using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

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
}
