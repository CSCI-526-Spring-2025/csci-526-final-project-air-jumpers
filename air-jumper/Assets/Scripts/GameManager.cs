using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] levelPrefabs; // Assign level prefabs in the Inspector
    private int currentLevelIndex = 0;
    private GameObject activeLevel;

    public PlayerMovement playerManager;

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    else
    {
        Destroy(gameObject);
    }
}
public void Start()
{
    LoadLevel(currentLevelIndex);
}


    public void LoadLevel(int levelIndex)
    {
                Debug.Log("Loaded level: " + levelIndex);

        if (activeLevel != null)
        {
            Destroy(activeLevel); // Remove the previous level
        }

        if (levelIndex < levelPrefabs.Length)
        {
            activeLevel = Instantiate(levelPrefabs[levelIndex], Vector3.zero, Quaternion.identity);
            currentLevelIndex = levelIndex;
            //ResetCheckpointToNewGround();

            BuildingInventoryManager.Instance.Clear();
            playerManager.Restart();
            FindObjectOfType<GameOverManager>().CancelGameOverTimer();
        }
        else
        {
            Debug.Log("All levels completed!");
        }
    }

    private void ResetCheckpointToNewGround()
    {
        GameObject newGround = activeLevel.transform.Find("BoundaryWalls/Ground").gameObject;
        if (newGround != null)
        {
            CheckpointManager.Instance.SetCheckpoint(newGround.transform.position);
        }
        else
        {
            Debug.LogError("Ground object not found in the new level!");
        }
    }

public void AdvanceToNextLevel()
{
    Debug.Log("Attempting to load next level: " + (currentLevelIndex + 1));

    if (currentLevelIndex + 1 < levelPrefabs.Length)
    {
        LoadLevel(currentLevelIndex + 1);
    }
    else
    {
        Debug.Log("No more levels");
    }
}

}
