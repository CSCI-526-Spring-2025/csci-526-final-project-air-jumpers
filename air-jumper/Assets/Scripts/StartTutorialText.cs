using UnityEngine;

public class KeyHintSprite : MonoBehaviour
{
    public GameObject keyHints; // ������ʾ�� GameObject ������
    public string currentLevel;
    void Start()
    {
        // 5 �������
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
        keyHints.SetActive(false);
    }
}