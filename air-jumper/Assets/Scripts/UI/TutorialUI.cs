using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainUIController : MonoBehaviour
{
    public GameObject mainUI;
    public Animator animator;
    public Button closeButton;

    private bool isUIOpen = false;

    void Start()
    {
        mainUI.SetActive(false);
        closeButton.onClick.AddListener(CloseUI);
        animator.SetBool("isClosing", false);
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!isUIOpen)
            {
                SpriteRenderer sr = mainUI.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = 0.75f;
                    sr.color = c;
                }
                mainUI.SetActive(true);
                
                animator.Play("Idle_Open"); 
                isUIOpen = true;

                Time.timeScale = 0f;

                FindObjectOfType<LevelSelectionMenu>()?.CloseLevelSelection();
            }
            else
            {
                ClosePanel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isUIOpen)
            {
                ClosePanel();
            }
        }
    }

    void CloseUI()
    {
        animator.SetBool("isClosing", true); 
        StartCoroutine(WaitAndClose());
    }

    IEnumerator WaitAndClose()
    {
        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(2f); ; 
        mainUI.SetActive(false);
        animator.SetBool("isClosing", false); 
        isUIOpen = false;
    }

    public void ClosePanel()
    {
        Time.timeScale = 1f;
        mainUI.SetActive(false);
        isUIOpen = false;
    }
}
