using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/BuildingMaterialCollectible")]
public class BuildingMaterialCollectibleScriptable : CollectibleScriptable
{
    public BuildingMaterialScriptable buildingMaterialData;

    private void OnEnable()
    {
        category = CollectibleCategory.BuildingMaterial;
    }

    public override void ApplyEffect(GameObject player)
    {
        BuildingInventoryManager buildingInventoryManager = BuildingInventoryManager.Instance;
        buildingInventoryManager.AddBuildingMaterial(buildingMaterialData, amount);
    }
}
