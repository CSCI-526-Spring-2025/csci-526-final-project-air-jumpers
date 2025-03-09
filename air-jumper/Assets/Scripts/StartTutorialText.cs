using UnityEngine;

public class KeyHintSprite : MonoBehaviour
{
    public GameObject keyHints; // 按键提示的 GameObject 父对象

    void Start()
    {
        // 5 秒后隐藏
        Invoke("HideHints", 7f);
    }

    void HideHints()
    {
        keyHints.SetActive(false);
    }
}