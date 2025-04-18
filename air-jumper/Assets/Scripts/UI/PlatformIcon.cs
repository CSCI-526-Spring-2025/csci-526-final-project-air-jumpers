using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlatformIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tmp;
    public GameObject tooltip;
    public Image tooltipBackground;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI tooltipUI = tooltip.GetComponent<TextMeshProUGUI>();
        if (tooltipUI)
        {
            tooltipUI.text = "Press SPACE in the air to place a normal platform";
        }
    }

    void Update()
    {
        if (tooltip.activeInHierarchy)
        {

            RectTransform textRect = tooltip.GetComponent<RectTransform>();
            RectTransform bgRect = tooltipBackground.GetComponent<RectTransform>();

            bgRect.transform.SetAsLastSibling();
            textRect.transform.SetAsLastSibling();

            bgRect.sizeDelta = textRect.sizeDelta + new Vector2(8, 10);
            bgRect.pivot = textRect.pivot;
            bgRect.anchorMin = textRect.anchorMin;
            bgRect.anchorMax = textRect.anchorMax;
            bgRect.anchoredPosition = textRect.anchoredPosition;

            bgRect.anchoredPosition = textRect.anchoredPosition - new Vector2(0, 5);
        }
    }

    public void SetPlatformCount(int count)
    {
        if (tmp != null)
        {
            tmp.text = $"{count}";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
        tooltipBackground.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
        tooltipBackground.gameObject.SetActive(false);
    }
}
