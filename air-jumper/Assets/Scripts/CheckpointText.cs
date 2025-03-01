using UnityEngine;
using TMPro; // 如果使用 TextMeshPro，则改为 using TMPro;

public class Checkpoint2D : MonoBehaviour
{
    public TMP_Text checkpointText;  
    public string message = "Checkpoint Reached!"; 

    private void Start()
    {
        checkpointText.gameObject.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            checkpointText.text = message; 
            checkpointText.gameObject.SetActive(true); 
            Invoke("HideText", 7f); 
        }
    }

    void HideText()
    {
        checkpointText.gameObject.SetActive(false); 
    }
}

