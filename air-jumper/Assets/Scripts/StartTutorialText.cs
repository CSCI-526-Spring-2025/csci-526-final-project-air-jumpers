using UnityEngine;

public class KeyHintSprite : MonoBehaviour
{
    public GameObject keyHints;
    public string currentLevel;
    void Start()
    {
        if (currentLevel == "Tutorial1")
        {
            keyHints.SetActive(true);
            Invoke("HideHints", 7f);
        }
        else
        {
            HideHints();
        }
    }

    void HideHints()
    {
        keyHints?.SetActive(false);
    }
}