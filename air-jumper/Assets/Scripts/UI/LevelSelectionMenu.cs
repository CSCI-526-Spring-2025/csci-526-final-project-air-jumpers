using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenu : MonoBehaviour
{
    public GameObject levelSelectionPanel; 

    private bool isMenuOpen = false;

    public void ToggleLevelSelection()
    {
        isMenuOpen = !isMenuOpen;
        levelSelectionPanel.SetActive(isMenuOpen);
    }

    private void Start()
    {
        levelSelectionPanel.SetActive(false); 
    }

    public void OpenLevelSelection()
    {
        levelSelectionPanel.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        isMenuOpen = false;
        levelSelectionPanel.SetActive(false);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1;
        BuildingInventoryManager.Instance?.Clear();
    }
}
