using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage; 
    private bool isTutorialActive = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            ToggleTutorial();
        }
    }

    void ToggleTutorial()
    {
        isTutorialActive = !isTutorialActive;
        tutorialImage.SetActive(isTutorialActive); 

        if (isTutorialActive)
        {
            Time.timeScale = 0f; 
        }
        else
        {
            Time.timeScale = 1f; 
        }
    }
}
