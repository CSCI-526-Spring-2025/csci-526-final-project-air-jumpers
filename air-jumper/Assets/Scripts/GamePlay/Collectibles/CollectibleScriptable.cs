using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleCategory
{
    Skill,
    BuildingMaterial
}

public abstract class CollectibleScriptable : ScriptableObject
{
    public CollectibleCategory type;
    public GameObject prefab;

    public abstract void ApplyEffect(GameObject player);
}


[CreateAssetMenu(menuName = "Collectibles/SkillCollectible")]
public class SkillCollectibleScriptable : CollectibleScriptable
{
    private void OnEnable()
    {
        type = CollectibleCategory.Skill;
    }

    public override void ApplyEffect(GameObject player)
    {

    }
}

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

    }
}