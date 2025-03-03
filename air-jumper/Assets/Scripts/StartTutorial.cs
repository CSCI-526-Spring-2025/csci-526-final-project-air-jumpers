using UnityEngine;
using UnityEngine.UI;

public class KeyHintUI : MonoBehaviour
{
    public GameObject hintPanel; 

    void Start()
    {
        
        Invoke("HideHints", 7f);
    }

    void HideHints()
    {
        hintPanel.SetActive(false);
    }
}