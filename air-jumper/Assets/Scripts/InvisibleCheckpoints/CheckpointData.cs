using UnityEngine;

/// <summary>
/// Represents data for a single checkpoint, including its position, 
/// the time the player reached it, and the time elapsed since the last checkpoint.
/// </summary>
[System.Serializable]
public class CheckpointData
{
    /// <summary>
    /// The position of the checkpoint in the game world.
    /// </summary>
    public Vector3 CheckpointPosition { get; private set; }

    /// <summary>
    /// The time (in seconds) when the player reached this checkpoint, relative to the level start.
    /// </summary>
    public float TimeReached { get; private set; }

    /// <summary>
    /// The time (in seconds) elapsed since the player reached the previous checkpoint.
    /// </summary>
    public float TimeSinceLastCheckpoint { get; private set; }

    /// <summary>
    /// The total number of jumps the player has made up to this checkpoint.
    /// </summary>
    public int TotalJumps { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckpointData"/> class.
    /// </summary>
    /// <param name="position">The position of the checkpoint.</param>
    /// <param name="timeReached">The time the player reached this checkpoint.</param>
    /// <param name="timeSinceLastCheckpoint">The time elapsed since the last checkpoint.</param>
    /// <param name="totalJumps">The total number of jumps the player has made up to this checkpoint.</param>
    public CheckpointData(Vector3 position, float timeReached, float timeSinceLastCheckpoint, int totalJumps)
    {
        CheckpointPosition = position;
        TimeReached = timeReached;
        TimeSinceLastCheckpoint = timeSinceLastCheckpoint;
        TotalJumps = totalJumps;
    }
}