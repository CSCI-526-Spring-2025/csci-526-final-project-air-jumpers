using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/BuildingMaterialCollectible")]
public class BuildingMaterialCollectibleScriptable : CollectibleScriptable
{
    public BuildingMaterialScriptable buildingMaterialData;

    private void OnEnable()
    {
        type = CollectibleCategory.BuildingMaterial;
    }

    public override void ApplyEffect(GameObject player)
    {
        BuildingInventoryManager buildingInventoryManager = BuildingInventoryManager.Instance;
        buildingInventoryManager.AddBuildingMaterial(buildingMaterialData);
    }
}
