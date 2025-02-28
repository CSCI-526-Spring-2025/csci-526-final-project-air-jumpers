using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDrawer : MonoBehaviour
{
    public Transform panel;
    public GameObject inventoryItemPrefab;

    private Dictionary<BuildingMaterialScriptable, GameObject> uiCollection = new Dictionary<BuildingMaterialScriptable, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        BuildingInventoryManager.Instance.OnMaterialUpdated += OnMaterialUpdatedListener;
    }

    private void OnDestroy()
    {
        BuildingInventoryManager.Instance.OnMaterialUpdated -= OnMaterialUpdatedListener;
    }

    private void OnMaterialUpdatedListener(BuildingMaterialScriptable material, int newCount)
    {
        GridLayoutGroup grid = panel.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            if (uiCollection.ContainsKey(material))
            {
                GameObject uiObject = uiCollection[material];
                if (newCount == 0)
                {
                    Destroy(uiObject);
                }
                else
                {
                    BuildingMaterialButton buttonController = uiObject.GetComponent<BuildingMaterialButton>();
                    buttonController.UpdateCount(newCount);
                }
            }
            else
            {
                GameObject uiObject = Instantiate(inventoryItemPrefab, panel, false);
                BuildingMaterialButton buttonController = uiObject.GetComponent<BuildingMaterialButton>();
                buttonController.materialData = material;
                buttonController.UpdateCount(newCount);

                uiCollection[material] = uiObject;
            }
        }
    }
}
