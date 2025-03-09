using UnityEngine;

public class KeyHintSprite : MonoBehaviour
{
    public GameObject keyHints; // ������ʾ�� GameObject ������

    void Start()
    {
        // 5 �������
        Invoke("HideHints", 7f);
    }

    void HideHints()
    {
        keyHints.SetActive(false);
    }
}