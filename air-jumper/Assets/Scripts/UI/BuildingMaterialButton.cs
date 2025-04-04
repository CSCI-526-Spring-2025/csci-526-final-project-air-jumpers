using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingMaterialButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI text;
    public Button button;
    public GameObject tooltip;
    public Image tooltipBackground;

    public float buttonWidth = 75;

    private int inventoryCount = 0;
    public BuildingMaterialScriptable materialData;

    private GameObject buildingBlock;
    private GameObject blockInstance;

    private RectTransform canvasRect;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (text != null)
        {
            text.text = $"{inventoryCount}";
        }

        if (button != null)
        {
            GameObject icon = Instantiate(materialData.prefab, button.transform, false);
            icon.transform.localScale *= buttonWidth / 3;
        }

        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        player = GameObject.FindWithTag("Player");

        TextMeshProUGUI tooltipUI = tooltip.GetComponent<TextMeshProUGUI>();
        if (tooltipUI)
        {
            tooltipUI.text = materialData.description;
        }
    }

    public void UpdateCount(int count)
    {
        inventoryCount = count;
        if (text != null)
        {
            text.text = $"{count}";
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        buildingBlock = new GameObject("BuildingMaterial");

        blockInstance = Instantiate(materialData.prefab, buildingBlock.transform, false);
        blockInstance.tag = "Platform";

        if (player != null)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), blockInstance.GetComponent<Collider2D>(), true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRect,
            eventData.position,
            Camera.main,
            out worldPos
        );
        buildingBlock.transform.position = worldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        if (CanPlaceMaterial(worldPos, materialData))
        {
            BuildingInventoryManager.Instance.TryUsingMaterial(materialData, 1);
            if (player != null)
            {
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), blockInstance.GetComponent<Collider2D>(), false);
            }

            SpriteRenderer renderer = blockInstance.GetComponent<SpriteRenderer>();

            BuildingPlatformManager canceler = blockInstance.AddComponent<BuildingPlatformManager>();
            canceler.materialInstance = buildingBlock;
            canceler.materialData = materialData;

            BuildingInventoryManager.Instance.PlacePlatform(blockInstance);

            if (renderer != null)
            {
                renderer.sortingOrder = 0;
            }
        }
        else
        {
            Destroy(buildingBlock);
        }
    }

    private bool CanPlaceMaterial(Vector2 position, BuildingMaterialScriptable materialData)
    {
        // TODO: Check wether inside the game canvas
        Collider2D[] overlap = Physics2D.OverlapBoxAll(position, materialData.snapSize, 0).Where(c =>
        {
            return !c.CompareTag("Checkpoint");
        }).ToArray();

        return overlap.Length == 1;
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
