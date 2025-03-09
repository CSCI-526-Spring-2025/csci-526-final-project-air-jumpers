using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingMaterialButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI text;
    public Button button;
    
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
            
            BuildingMaterialCanceler canceler = blockInstance.AddComponent<BuildingMaterialCanceler>();
            canceler.materialInstance = buildingBlock;
            canceler.materialData = materialData;

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
        Collider2D[] overlap = Physics2D.OverlapBoxAll(position, materialData.snapSize, 0);
        return overlap.Length == 1;
    }
}
