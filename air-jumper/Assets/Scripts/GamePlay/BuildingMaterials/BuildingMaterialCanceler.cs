using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMaterialCanceler : MonoBehaviour
{
    public bool cancalable = true;

    public GameObject canvas;
    public GameObject redoButton;

    public GameObject materialInstance;
    public BuildingMaterialScriptable materialData;

    private void CreateCanvas()
    {
        canvas = new GameObject("BuildingMaterialCanvas");

        Canvas canvasInstance = canvas.AddComponent<Canvas>();
        canvasInstance.renderMode = RenderMode.WorldSpace;
        canvasInstance.sortingOrder = 100;

        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        canvas.transform.SetParent(transform, false);

        RectTransform rt = canvas.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(2, 2);

        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
    }

    private void CancelMaterialPlacement()
    {
        BuildingInventoryManager.Instance.AddBuildingMaterial(materialData);
        Destroy(materialInstance);
    }

    private void CreateButton()
    {
        if (canvas == null)
        {
            CreateCanvas();
        }

        if (redoButton == null)
        {
            redoButton = new GameObject("RedoButton");
            redoButton.transform.SetParent(canvas.transform, false);

            RectTransform rt = redoButton.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 50);
            rt.anchoredPosition = Vector2.zero;

            Button buttonInstance = redoButton.AddComponent<Button>();
            buttonInstance.onClick.AddListener(() =>
            {
                CancelMaterialPlacement();
            });

            GameObject text = new GameObject("RedoButtonText");
            text.transform.SetParent(buttonInstance.transform, false);

            RectTransform textRT = text.AddComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;

            TextMeshProUGUI textInstance = text.AddComponent<TextMeshProUGUI>();
            textInstance.text = "redo";
            textInstance.color = Color.black;
            textInstance.alignment = TextAlignmentOptions.Center;
            textInstance.fontSize = 0.5f;
        }
    }

    private void OnMouseEnter()
    {
        if (!cancalable)
        {
            return;
        }

        CreateButton();
    }

    private void OnMouseExit()
    {
        if (redoButton != null)
        {
            Destroy(redoButton);
        }
    }
}
