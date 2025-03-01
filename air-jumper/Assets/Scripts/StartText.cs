using UnityEngine;
using TMPro; 

public class StartMessage : MonoBehaviour
{
    public TMP_Text startText;  
    public float displayDuration = 7f; 
    private void Start()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(true); 
            Invoke("HideText", displayDuration);  
        }
    }

    void HideText()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(false); 
        }
    }
}
