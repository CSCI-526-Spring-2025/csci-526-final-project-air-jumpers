using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the Replay Button functionality, allowing the player to restart the current level.
/// </summary>
public class ReplayController : MonoBehaviour
{
    /// <summary>
    /// Called when the Replay Button is clicked.
    /// Restarts the current level and clears any relevant game state.
    /// </summary>
    public void ReplayGame()
    {
        // Resume the game in case it was paused
        Time.timeScale = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Clear the building inventory if applicable
        BuildingInventoryManager.Instance?.Clear();
    }
}