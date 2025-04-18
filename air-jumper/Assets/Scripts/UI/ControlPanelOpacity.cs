using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlPanelOpacity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (image == null)
        {
            image = gameObject.GetComponent<Image>();
        }
        Color color = image.color;
        color.a = 0.8f;
        image.color = color;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
    }
}
