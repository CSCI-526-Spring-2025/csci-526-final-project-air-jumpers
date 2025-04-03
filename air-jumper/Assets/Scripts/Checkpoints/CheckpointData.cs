using UnityEngine;

[System.Serializable]
public class CheckpointData
{
    public Vector3 position;
    public float timeReached;
    public float timeSinceLastCheckpoint;

    public CheckpointData(Vector3 pos, float reachedTime, float durationFromPrevious)
    {
        position = pos;
        timeReached = reachedTime;
        timeSinceLastCheckpoint = durationFromPrevious;
    }
}
